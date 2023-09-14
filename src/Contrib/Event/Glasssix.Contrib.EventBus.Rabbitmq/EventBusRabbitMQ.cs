using Autofac;
using EventBus;
using Glasssix.Contrib.EventBus.Abstractions;
using Glasssix.Contrib.EventBus.Events;
using Glasssix.Contrib.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Glasssix.Contrib.EventBus.Rabbitmq
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        //private static ConcurrentQueue<IModel> _basicChannel = new ConcurrentQueue<IModel>();
        //private readonly IModel _channel;
        private const string AUTOFAC_SCOPE_NAME = "Scope_tag_event";

        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IDefaultRabbitMQConnection _persistentConnection;
        private readonly int _retryCount;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope scope;
        private string? _queueName;
        private string Exchanges = "event_bus";//默认交换机

        public EventBusRabbitMQ(IDefaultRabbitMQConnection persistentConnection, ILogger<EventBusRabbitMQ> logger,
        ILifetimeScope autofac, IEventBusSubscriptionsManager subsManager, string? exchange = null, string? queueName = null, int retryCount = 5)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _queueName = queueName;
            Exchanges = exchange!;
            scope = autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
            _retryCount = retryCount;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        #region 路由模式

        #region Push

        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="event"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Publish(IntegrationEvent @event)
        {
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            policy.Execute(() =>
            {
                using var channel = _persistentConnection.CreateModel();
                channel.ExchangeDeclare(exchange: Exchanges, type: ExchangeType.Direct, durable: true, autoDelete: false); //创建消息交换机
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent
                _logger.LogDebug("Publishing Event to RabbitMQ: {EventId}", @event.Id);

                channel.BasicPublish(
                    exchange: Exchanges,
                    routingKey: @event.GetType().Name,
                     mandatory: true,
                    basicProperties: properties,
                    body: System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType()));
            });
        }

        #endregion Push

        #region Subscribe

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <exception cref="NotImplementedException"></exception>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            var channel = CreateConsumerChannel(eventName);
            DoInternalSubscription(channel, eventName);

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", typeof(T).GetGenericTypeName(), typeof(TH).GetGenericTypeName());

            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume(channel);
        }

        /// <summary>
        /// 根据事件名称订阅Hander
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).GetGenericTypeName());
            var channel = CreateConsumerChannel(eventName);
            DoInternalSubscription(channel, eventName);
            _subsManager.AddDynamicSubscription<TH>(eventName);
            StartBasicConsume(channel);
        }

        #endregion Subscribe

        #region QueueBind

        /// <summary>
        /// 绑定默认消费交换机
        /// </summary>
        /// <param name="eventName"></param>
        private void DoInternalSubscription(IModel _consumerChannel, string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }
                ////死信队列绑定死信交换机
                //_consumerChannel.QueueBind(_queueName + "_dlx", ExchangesDlx, routingKey: eventName + "_dlx");
                //消息队列绑定消息交换机
                _consumerChannel.QueueBind(queue: _queueName, exchange: Exchanges, routingKey: eventName);
            }
        }

        #endregion QueueBind

        #region Unsubscribe

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <exception cref="NotImplementedException"></exception>
        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        /// <summary>
        /// 根据事件名称取消订阅Hander
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        #endregion Unsubscribe

        #region Consumer

        /// <summary>
        /// 创建默认消费信道
        /// </summary>
        /// <returns></returns>
        private IModel CreateConsumerChannel(string rooutingkey)
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            _logger.LogDebug("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            ////创建死信交换机
            //channel.ExchangeDeclare(ExchangesDlx, type: ExchangeType.Direct, durable: true, autoDelete: false);
            ////创建死信队列
            //channel.QueueDeclare(_queueName + "_dlx", durable: true, exclusive: false, autoDelete: false);
            //创建消息交换机
            channel.ExchangeDeclare(exchange: Exchanges, type: ExchangeType.Direct, durable: true, autoDelete: false);
            //创建消息队列
            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.BasicQos(0, 10, true);//限流 防止消费者暴毙
            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogDebug(ea.Exception, "Recreating RabbitMQ consumer channel");
                channel.Dispose();
                channel = CreateConsumerChannel(rooutingkey);
                StartBasicConsume(channel);
            };

            return channel;
        }

        #endregion Consumer

        #region EventMapMessage

        /// <summary>
        /// Event映射 解取消息体
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message, IModel _consumerChannel, BasicDeliverEventArgs eventArgs)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (scope.ResolveOptional(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = System.Text.Json.JsonSerializer.Deserialize(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent! });
                    }
                }
                _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            else
            {
                //_logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
                _consumerChannel.BasicNack(eventArgs.DeliveryTag, multiple: false, true);
                //_consumerChannel.BasicReject(eventArgs.DeliveryTag, false);
            }
        }

        #endregion EventMapMessage

        #region BindConsume

        /// <summary>
        /// 订阅消费者
        /// </summary>
        private void StartBasicConsume(IModel _consumerChannel)
        {
            _logger.LogDebug("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += async (sender, eventArgs) =>
                {
                    var eventName = eventArgs.RoutingKey;
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                    try
                    {
                        if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                            throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                        await ProcessEvent(eventName, message, _consumerChannel, eventArgs);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
                    }
                };

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();
            channel.QueueUnbind(queue: _queueName,
                exchange: Exchanges,
                routingKey: eventName);

            if (_subsManager.IsEmpty)
            {
                _queueName = string.Empty;
            }
        }

        #endregion BindConsume

        #endregion 路由模式

        #region 主题模式

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event"></param>
        public void PublishToTopic(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsTopicConnected)
            {
                _persistentConnection.TryConnectTopic();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var eventName = string.Empty;
            var exchanges = string.Empty;
            if (string.IsNullOrEmpty(@event.RoutingKey))
                eventName = @event.GetType().Name;
            else
                eventName = @event.RoutingKey;

            if (string.IsNullOrEmpty(@event.Exchanges))
                exchanges = Exchanges;
            else
                exchanges = @event.Exchanges;

            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using var channel = _persistentConnection.CreateTopicModel();
            channel.ExchangeDeclare(@event.Exchanges, "topic", false);
            channel.QueueDeclare(@event.Queue, false, false, false, null);
            channel.QueueBind(@event.Queue, @event.Exchanges, @event.RoutingKey);

            _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                if (@event.DelayTime.TotalMilliseconds > 0)
                    properties.Headers = new Dictionary<string, object>()
                {
                    {  "x-delay", @event.DelayTime.TotalMilliseconds }
                };

                _logger.LogTrace("Publishing TopicEvent to RabbitMQ: {EventId}", @event.Id);
                channel.BasicPublish(
                    exchange: exchanges,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType()));
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="exchanges"></param>
        /// <param name="queueName"></param>
        /// <param name="routingKey"></param>
        public void SubscribeToTopic<T, TH>(string exchanges, string queueName, string routingKey)
                 where T : IntegrationEvent
                 where TH : IIntegrationEventHandler<T>
        {
            var _consumerTopicChannel = CreateTopicConsumerChannel(exchanges, queueName);
            DoInternalTopicSubscription(exchanges, queueName, routingKey, _consumerTopicChannel);
            _logger.LogInformation("TopicSubscribing to event {EventName} with {EventHandler}", queueName, routingKey);
            _subsManager.AddTopicSubscription<T, TH>(routingKey);
            StartBasicTopicConsume(routingKey, queueName, _consumerTopicChannel);
        }

        /// <summary>
        /// 绑定交换机
        /// </summary>
        /// <param name="exchanges"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        private IModel CreateTopicConsumerChannel(string exchanges, string queueName)
        {
            if (!_persistentConnection.IsTopicConnected)
            {
                _persistentConnection.TryConnectTopic();
            }

            _logger.LogDebug("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateTopicModel();

            channel.ExchangeDeclare(exchanges, "topic", false);//主题模式

            channel.QueueDeclare(queue: queueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            return channel;
        }

        /// <summary>
        /// 绑定主题
        /// </summary>
        /// <param name="exchanges"></param>
        /// <param name="queueName"></param>
        /// <param name="routingKey"></param>
        /// <param name="_consumerTopicChannel"></param>
        private void DoInternalTopicSubscription(string exchanges, string queueName, string routingKey, IModel _consumerTopicChannel)
        {
            if (!_persistentConnection.IsTopicConnected)
            {
                _persistentConnection.TryConnectTopic();
            }

            _consumerTopicChannel.QueueBind(queue: queueName,
                                exchange: exchanges,
                                routingKey: routingKey);
        }

        /// <summary>
        /// Event映射 解取消息体
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessTopicEvent(string routingKey, string eventName, string message, IModel _consumerTopicChannel, BasicDeliverEventArgs eventArgs)
        {
            _logger.LogTrace("Processing Topic RabbitMQ event: {EventName}", routingKey);

            if (_subsManager.HasSubscriptionsForEvent(routingKey))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(routingKey);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (scope.ResolveOptional(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(handler.GetGenericTypeName().Replace("Handler", ""));
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent! });
                    }
                }
                _consumerTopicChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
                //_consumerTopicChannel.BasicNack(eventArgs.DeliveryTag, multiple: false, true);
            }
        }

        /// <summary>
        /// 订阅消费者
        /// </summary>
        private void StartBasicTopicConsume(string routingKey, string queueName, IModel _consumerTopicChannel)
        {
            _logger.LogDebug("Starting RabbitMQ basic consume");

            if (_consumerTopicChannel != null)
            {
                //var consumer = new AsyncEventingBasicConsumer(_consumerTopicChannel);
                var consumer = new EventingBasicConsumer(_consumerTopicChannel);

                consumer.Received += async (sender, eventArgs) =>
                {
                    var eventName = eventArgs.RoutingKey;
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                    try
                    {
                        if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                        {
                            throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                        }

                        await ProcessTopicEvent(routingKey, eventName, message, _consumerTopicChannel, eventArgs);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
                    }
                };

                _consumerTopicChannel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        #endregion 主题模式

        #region 延时模式

        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="event"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void PublishToDelay(IntegrationEvent @event)
        {
            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            policy.Execute(() =>
            {
                using var channel = _persistentConnection.CreateModel();
                var argMaps = new Dictionary<string, object>()
                {
                    {"x-delayed-type", "direct"}
                };
                channel.ExchangeDeclare(exchange: Exchanges + "_delay", type: "x-delayed-message", durable: true, autoDelete: false, argMaps); //创建延时队列
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent
                if (@event.DelayTime.TotalMilliseconds > 0)
                    properties.Headers = new Dictionary<string, object>()
                {
                    {  "x-delay",@event.DelayTime.TotalMilliseconds }
                };

                _logger.LogDebug("Publishing  DelayEvent to RabbitMQ: {EventId}", @event.Id);

                channel.BasicPublish(
                    exchange: Exchanges + "_delay",
                    routingKey: @event.GetType().Name,
                     mandatory: true,
                    basicProperties: properties,
                    body: System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType()));
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <exception cref="NotImplementedException"></exception>
        public void SubscribeToDelay<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            var channel = CreateConsumerDelayChannel(eventName);
            DoInternalSubscriptionDelay(channel, eventName);

            _logger.LogInformation("Subscribing to DelayEvent {EventName} with {EventHandler}", typeof(T).GetGenericTypeName(), typeof(TH).GetGenericTypeName());

            _subsManager.AddSubscription<T, TH>();
            StartBasicConsumeDelay(channel);
        }

        /// <summary>
        /// 创建默认消费信道
        /// </summary>
        /// <returns></returns>
        private IModel CreateConsumerDelayChannel(string rooutingkey)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogDebug("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            ////创建死信交换机
            //channel.ExchangeDeclare(ExchangesDlx, type: ExchangeType.Direct, durable: true, autoDelete: false);
            ////创建死信队列
            //channel.QueueDeclare(_queueName + "_dlx", durable: true, exclusive: false, autoDelete: false);

            var argMaps = new Dictionary<string, object>()
            {
                {"x-delayed-type", "direct"}
            };
            //创建消息交换机
            channel.ExchangeDeclare(exchange: Exchanges + "_delay", type: "x-delayed-message", durable: true, autoDelete: false, argMaps);
            //创建消息队列
            channel.QueueDeclare(queue: _queueName + "_delay", durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.BasicQos(0, 100, true);//限流 防止消费者暴毙
            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                channel.Dispose();
                channel = CreateConsumerDelayChannel(rooutingkey);
                StartBasicConsumeDelay(channel);
            };

            return channel;
        }

        /// <summary>
        /// 绑定默认消费交换机
        /// </summary>
        /// <param name="eventName"></param>
        private void DoInternalSubscriptionDelay(IModel _consumerChannel, string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }
                ////死信队列绑定死信交换机
                //_consumerChannel.QueueBind(_queueName + "_dlx", ExchangesDlx, routingKey: eventName + "_dlx");
                //消息队列绑定消息交换机
                _consumerChannel.QueueBind(queue: _queueName + "_delay", exchange: Exchanges + "_delay", routingKey: eventName);
            }
        }

        /// <summary>
        /// Event映射 解取消息体
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEventToDelay(string eventName, string message, IModel _consumerChannel, BasicDeliverEventArgs eventArgs)
        {
            _logger.LogDebug("Processing RabbitMQ event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (scope.ResolveOptional(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = System.Text.Json.JsonSerializer.Deserialize(message, eventType);
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent! });
                    }
                }
                _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            }
            else
            {
                //_logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
                _consumerChannel.BasicNack(eventArgs.DeliveryTag, multiple: false, true);
                //_consumerChannel.BasicReject(eventArgs.DeliveryTag, false);
            }
        }

        /// <summary>
        /// 订阅消费者
        /// </summary>
        private void StartBasicConsumeDelay(IModel _consumerChannel)
        {
            _logger.LogDebug("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += async (sender, eventArgs) =>
                {
                    var eventName = eventArgs.RoutingKey;
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                    try
                    {
                        if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                            throw new InvalidOperationException($"Fake exception requested: \"{message}\"");

                        await ProcessEventToDelay(eventName, message, _consumerChannel, eventArgs);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
                    }
                };

                _consumerChannel.BasicConsume(
                    queue: _queueName + "_delay",
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsumeDelay can't call on _consumerChannel == null");
            }
        }

        #endregion 延时模式

        #region Ex

        public void Dispose()
        {
            _subsManager.Clear();
            _persistentConnection.Dispose();
        }

        public void HealthyCheck()
        {
            return;
        }

        #endregion Ex
    }
}