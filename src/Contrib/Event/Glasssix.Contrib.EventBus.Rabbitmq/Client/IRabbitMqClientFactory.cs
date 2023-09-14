namespace Glasssix.Contrib.EventBus.Rabbitmq.Client
{
    public interface IRabbitMqClientFactory
    {
        /// <summary>
        /// 推送
        /// </summary>
        /// <param name="input">消息内容</param>
        /// <param name="routingKey">key</param>
        void Publish(string input, string routingKey);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="quque">主题</param>
        /// <param name="routingkey">key</param>
        void Subscription(string quque, string routingkey);
    }
}