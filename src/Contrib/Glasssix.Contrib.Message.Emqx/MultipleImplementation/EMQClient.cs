using Glasssix.Contrib.Message.Emqx.Option;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Message.Emqx.MultipleImplementation
{
    public class EMQClient : IMessageQueueClient
    {
        protected MQTTnet.Client.IMqttClient? _client;

        private readonly MqttOptions _options;
        public EMQClient(IOptions<MqttOptions> options)
        {
            _options = options.Value;
        }

        public MQTTnet.Client.IMqttClient Connnection => _client!;

        public async Task ReConnnectionAsync()
        {
            MQTTnet.Client.IMqttClient mqttClient = new MqttFactory().CreateMqttClient();

            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithClientId(_options.ClientId)
                .WithTimeout(TimeSpan.FromSeconds(30))
                .WithCleanSession(true)
                .WithTcpServer(_options.Ip, _options.Port)
                .WithCredentials(_options.Username, _options.Password);

            var option = mqttClientOptions.Build();

            await mqttClient.ConnectAsync(option, CancellationToken.None);

            _client = mqttClient;
        }

        public Task<MqttClientPublishResult> PublishAsync(string topic, string body)
        {
            return _client.PublishStringAsync(topic, body);
        }
    }
}
