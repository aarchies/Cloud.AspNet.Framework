using Glasssix.Contrib.Caching.Enumerations;
using Glasssix.Contrib.Caching.Options;
using Microsoft.Extensions.Caching.Memory;

namespace Glasssix.BuildingBlocks.Caching.MultilevelCache.Options
{
    /// <summary>
    /// 多级缓存选项
    /// </summary>
    public class MultilevelCacheGlobalOptions : MemoryCacheOptions
    {
        /// <summary>
        /// 内存默认有效时间配置
        /// </summary>
        public CacheEntryOptions? CacheEntryOptions { get; set; }

        public CacheOptions GlobalCacheOptions { get; set; } = new()
        {
            CacheKeyType = CacheKeyType.TypeName
        };

        /// <summary>
        /// 获取或设置订阅密钥的前缀
        /// </summary>
        public string SubscribeKeyPrefix { get; set; } = string.Empty;

        public SubscribeKeyType SubscribeKeyType { get; set; } = SubscribeKeyType.ValueTypeFullNameAndKey;
    }
}