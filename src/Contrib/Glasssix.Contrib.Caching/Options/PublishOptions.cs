using System;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 发布选项
    /// </summary>
    public class PublishOptions : PubSubOptionsBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="uniquelyIdentifies"></param>
        public PublishOptions(Guid uniquelyIdentifies) : base(uniquelyIdentifies)
        {
        }

        /// <summary>
        /// V
        /// </summary>

        public object? Value { get; set; }
    }
}