using System;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 订阅 泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SubscribeOptions<T> : PubSubOptionsBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uniquelyIdentifies"></param>
        public SubscribeOptions(Guid uniquelyIdentifies) : base(uniquelyIdentifies)
        {
        }

        /// <summary>
        /// 是否发送者客户端
        /// </summary>
        public bool IsPublisherClient { get; set; }

        /// <summary>
        /// 值构造 泛型
        /// </summary>
        public T? Value { get; set; }
    }
}