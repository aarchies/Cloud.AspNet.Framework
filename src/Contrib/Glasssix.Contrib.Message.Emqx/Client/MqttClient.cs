using Glasssix.Contrib.Message.Emqx.C;
using Glasssix.Contrib.Message.Emqx.Meta;
using Glasssix.Contrib.Message.Emqx.Option;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Message.Emqx.Client
{
    public class MqttClient : IMqttClient
    {
        private readonly string _clientId;
        private readonly string _crtfile;
        private readonly string _keyfile;
        private readonly ILogger<MqttClient> _logger;
        private readonly string _node;
        private readonly string _password;
        private readonly int _port;
        private readonly string _username;
        private MQTTnet.Client.IMqttClient _client;
        private readonly List<string> topics = new List<string>();

        public MqttClient(ILoggerFactory logger, MqttOptions option)
        {
            _node = option.Ip!;
            _port = option.Port;
            _clientId = option.ClientId!;
            _username = option.Username!;
            _password = option.Password!;
            _crtfile = option.Crtfile!;
            _keyfile = option.Keyfile!;
            _logger = logger.CreateLogger<MqttClient>();
            _client = ConnectAsync().GetAwaiter().GetResult();
            _client.ApplicationMessageReceivedAsync += MqttServe_MsgReceive;
        }

        public event Action<MqttNetMessage>? ReciveMsg;


        public async Task<bool> PublishAsync(string topic, string body)
        {
            var result = await _client.PublishStringAsync(topic, body);
            return result.ReasonCode == MqttClientPublishReasonCode.Success;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task Subscribe(string topic)
        {
            AddTopic(topic);
            var mqttSubscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(topic); })
                .Build();

            await _client.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        }

        private async Task<MQTTnet.Client.IMqttClient> ConnectAsync()
        {
            var mqttClient = new MqttFactory().CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithClientId(_clientId).WithTimeout(TimeSpan.FromSeconds(30))
                .WithCleanSession(true)
                .WithTcpServer(_node, _port)
                .WithCredentials(_username, _password);

            if (!string.IsNullOrWhiteSpace(_crtfile) && !string.IsNullOrWhiteSpace(_keyfile))
            {
                var model = new List<X509Certificate>();
                var certificate = new X509Certificate(
                                         Certificate.GetCertificateFromPEMstring(
                                              File.ReadAllText(_crtfile),
                                              File.ReadAllText(_keyfile),
                                          ""));
                model.Add(certificate);
                mqttClientOptions.WithTls(new MqttClientOptionsBuilderTlsParameters()
                {
                    AllowUntrustedCertificates = true,
                    Certificates = model,
                    UseTls = true,
                });
            }
            var option = mqttClientOptions.Build();
            await mqttClient.ConnectAsync(option, CancellationToken.None);

            mqttClient.DisconnectedAsync += (arg) => MqttClient_DisconnectedAsync(arg, option);

            return mqttClient;
        }

        private async Task MqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs arg, MQTTnet.Client.MqttClientOptions options)
        {
            _logger.LogInformation("连接EMQ...");
            while (!_client.IsConnected)
            {
                try
                {
                    await _client.ConnectAsync(options, CancellationToken.None);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "");
                    await Task.Delay(5000);
                }
            }

            var topicBuilder = new MqttClientSubscribeOptionsBuilder();

            topics.Distinct().ToList().ForEach(topic =>
            {
                topicBuilder.WithTopicFilter(topic);
            });

            await _client.SubscribeAsync(topicBuilder.Build());

            _logger.LogInformation("成功连接到EMQ");
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private Task MqttServe_MsgReceive(MqttApplicationMessageReceivedEventArgs e)
        {
            ReciveMsg?.Invoke(new MqttNetMessage()
            {
                ClientId = e.ClientId,
                Topic = e.ApplicationMessage.Topic,
                Data = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)
            });
            return Task.CompletedTask;
        }

        private void AddTopic(string topic)
        {
            lock (topics)
            {
                topics.Add(topic);
            }
        }

        Task<MQTTnet.Client.IMqttClient> IMqttClient.GetMqttClientInterface()
        {
            return Task.FromResult(_client);
        }
    }
}