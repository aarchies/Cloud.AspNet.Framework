namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 组合缓存周期选项
    /// </summary>
    public class CombinedCacheEntryOptions
    {
        /// <summary>
        /// 分布式缓存入口选项
        /// </summary>
        public CacheEntryOptions? DistributedCacheEntryOptions { get; set; }

        /// <summary>
        /// 内存缓存入口选项
        /// </summary>
        public CacheEntryOptions? MemoryCacheEntryOptions { get; set; }
    }
}