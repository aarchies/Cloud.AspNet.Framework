using Glasssix.Contrib.Message.Emqx.MultipleImplementation.Attribuites.Option;
using Glasssix.Contrib.Message.Emqx.MultipleImplementation.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Message.Emqx.MultipleImplementation.Workers
{
    public sealed class EMQWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RouterHandlerTypeMap _routerHandlerTypeMap;
        private readonly ILogger<EMQWorker> _logger;
        private readonly IMessageQueueClient _client;

        public EMQWorker(
            IServiceProvider serviceProvider,
            RouterHandlerTypeMap routerHandlerTypeMap,
            ILogger<EMQWorker> logger,
            IMessageQueueClient client)
        {
            _serviceProvider = serviceProvider;
            _routerHandlerTypeMap = routerHandlerTypeMap;
            _logger = logger;
            _client = client;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            _logger.LogInformation("EMQ worker starting...");

            await _client.ReConnnectionAsync();
            _client.Connnection.ApplicationMessageReceivedAsync += async (e) =>
            {
                var type = _routerHandlerTypeMap.FirstOrDefault(x => x.Key.Regex.IsMatch(e.ApplicationMessage.Topic)).Value;

                if (type is null)
                {
                    _logger.LogError("The Topic:{_} No handler found.", e.ApplicationMessage);
                    return;
                }

                using var scope = _serviceProvider.CreateScope();

                var instance = (IMessageHandler)scope.ServiceProvider.GetRequiredService(type);

                var content = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

                try
                {
                    await instance.HandleAsync(new MqttNetMessage
                    {
                        ClientId = e.ClientId,
                        Topic = e.ApplicationMessage.Topic,
                        Data = content
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                }
            };

            foreach (var item in _routerHandlerTypeMap)
            {
                var topic = $"$share/${item.Key.Group}/" + item.Key.Topic;
                if (item.Key.Group == null)
                    topic = item.Key.Topic;

                var mqttSubscribeOptions = new MqttFactory()
                      .CreateSubscribeOptionsBuilder()
                      .WithTopicFilter(f => { f.WithTopic(topic); })
                      .Build();

                await _client.Connnection.SubscribeAsync(mqttSubscribeOptions, stoppingToken);
            }

            _client.Connnection.DisconnectedAsync += async (s) =>
            {
                _client.Connnection.Dispose();
                while (!_client.Connnection.IsConnected)
                {
                    _logger.LogInformation("EMQ重连...");
                    try
                    {
                        await ExecuteAsync(stoppingToken);
                    }
                    catch (Exception e)
                    {
                        await Task.Delay(15000);
                        _logger.LogError(e, "");
                    }
                }
            };
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

            _client.Connnection.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
