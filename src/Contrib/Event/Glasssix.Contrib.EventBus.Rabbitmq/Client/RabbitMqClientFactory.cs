using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Sockets;
using System.Text;

namespace Glasssix.Contrib.EventBus.Rabbitmq.Client
{
    public class RabbitMqClientFactory : IRabbitMqClientFactory
    {
        private readonly ILogger<RabbitMqClientFactory> _logger;
        private readonly IDefaultRabbitMQConnection _persistentConnection;
        private IModel _consumerChannel;

        public RabbitMqClientFactory(IDefaultRabbitMQConnection persistentConnection, ILogger<RabbitMqClientFactory> logger)
        {
            _consumerChannel = CreateConsumerChannel("", "");
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="event"></param>
        public void Publish(string input, string routingKey)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<Exception>()
                .Or<SocketException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                });

            using var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: "Device", ExchangeType.Topic);

            var sendBytes = Encoding.UTF8.GetBytes(input);

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                _logger.LogDebug("Publishing event to RabbitMQ: {EventId}");

                channel.BasicPublish(
                    exchange: "Device",
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: sendBytes);
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="eventName"></param>
        public void Subscription(string quque, string routingkey)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            _consumerChannel = CreateConsumerChannel(quque, routingkey);
            _consumerChannel.QueueBind(queue: quque,
                                exchange: "Device",
                                routingKey: routingkey);
            StartBasicConsume(quque);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var RoutingKey = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }
                //接取消息 eventName
                if (RoutingKey == "1")
                {
                }
                if (RoutingKey == "2")
                {
                }
                if (RoutingKey == "3")
                {
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        /// <summary>
        /// 创建消费者队列
        /// </summary>
        /// <returns></returns>
        private IModel CreateConsumerChannel(string queue, string routingkey)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: "Device", type: "direct");

            channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel(queue, routingkey);
                StartBasicConsume(queue);
            };

            return channel;
        }

        /// <summary>
        /// 订阅消费者
        /// </summary>
        private void StartBasicConsume(string queue)
        {
            _logger.LogDebug("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: queue,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }
    }
}