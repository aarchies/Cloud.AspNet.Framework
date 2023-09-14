namespace Glasssix.Contrib.EventBus.Rabbitmq.Option
{
    public class EventBusOptions
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string? Connection { get; set; }

        /// <summary>
        /// 交换机
        /// </summary>
        public string? Exchange { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string? Port { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string? QueueName { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public string? RetryCount { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
    }
}