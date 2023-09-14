using Glasssix.Contrib.EventBus.Events;

namespace Glasssix.Contrib.EventBus.Abstractions
{
    public interface IEventBus
    {
        #region 公平分发

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="event"></param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <typeparam name="TH">事件处理者</typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 根据事件名称订阅
        /// </summary>
        /// <typeparam name="TH">事件处理者</typeparam>
        /// <param name="eventName">事件名称</param>
        void SubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <typeparam name="TH">事件处理者</typeparam>
        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        /// <summary>
        ///根据事件名称取消订阅
        /// </summary>
        /// <typeparam name="TH">事件处理者</typeparam>
        /// <param name="eventName">事件名称</param>
        void UnsubscribeDynamic<TH>(string eventName)
                where TH : IDynamicIntegrationEventHandler;

        #endregion 公平分发

        #region 主题模式

        /// <summary>
        /// 主题模式发布
        /// </summary>
        /// <param name="event"></param>
        void PublishToTopic(IntegrationEvent @event);

        /// <summary>
        /// 主题模式订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="exchanges">交换机</param>
        /// <param name="queueName">主题</param>
        /// <param name="routingKey">key</param>
        void SubscribeToTopic<T, TH>(string exchanges, string queueName, string routingKey)
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

        #endregion 主题模式

        #region 延时模式

        /// <summary>
        /// 延时发布
        /// </summary>
        /// <param name="event"></param>
        void PublishToDelay(IntegrationEvent @event);

        /// <summary>
        /// 延时订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        void SubscribeToDelay<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

        #endregion 延时模式

        void HealthyCheck();
    }
}