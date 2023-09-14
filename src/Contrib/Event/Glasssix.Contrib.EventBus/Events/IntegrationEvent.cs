using System;
using System.Text.Json.Serialization;

namespace Glasssix.Contrib.EventBus.Events
{
    /// <summary>
    /// Envent消息记录基类
    /// </summary>
    public record IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        /// <summary>
        /// 消息标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 消息时间
        /// </summary>
        [JsonInclude]
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// 队列
        /// </summary>
        public string? Queue { get; set; }

        /// <summary>
        /// 队列RoutingKey
        /// </summary>
        public string? RoutingKey { get; set; }

        /// <summary>
        /// 交换机
        /// </summary>
        public string? Exchanges { get; set; }

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string? virtualHost { get; set; }

        /// <summary>
        /// 延时时间 手动赋值则启用当前事件延时
        /// </summary>
        public TimeSpan DelayTime { get; set; } = default;
    }
}