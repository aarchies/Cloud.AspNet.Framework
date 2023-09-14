namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 多级缓存配置选项
    /// </summary>
    public class MultilevelCacheOptions : CacheOptions
    {
        /// <summary>
        /// 内存默认有效时间配置
        /// 当内存缓存不存在时，当将结果新写入内存缓存时，将使用从分布式缓存获得的结果
        /// 如果未指定，则默认使用全局配置
        /// </summary>
        public CacheEntryOptions? MemoryCacheEntryOptions { get; set; }
    }
}