using Glasssix.Contrib.Caching.Enumerations;
using System;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 发布订阅选项
    /// </summary>
    public abstract class PubSubOptionsBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uniquelyIdentifies"></param>
        protected PubSubOptionsBase(Guid uniquelyIdentifies) => UniquelyIdentifies = uniquelyIdentifies;


        /// <summary>
        /// 发布Key
        /// </summary>
        public string Key { get; set; } = default!;

        /// <summary>
        ///设置选项
        /// </summary>
        public SubscribeOperation Operation { get; set; }

        /// <summary>
        /// 唯一标识符，用于确认发件人和订阅者是否为同一客户端
        /// </summary>
        public Guid UniquelyIdentifies { get; private set; }
    }
}