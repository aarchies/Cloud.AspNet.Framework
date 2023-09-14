using Glasssix.Contrib.Caching.Options;

namespace Glasssix.BuildingBlocks.Caching.MultilevelCache
{
    public class MultilevelCachePublish<T>
    {
        public MultilevelCachePublish()
        {
        }

        public MultilevelCachePublish(T? value, CacheEntryOptions? cacheEntryOptions = null) : this()
        {
            Value = value;
            CacheEntryOptions = cacheEntryOptions;
        }

        public CacheEntryOptions? CacheEntryOptions { get; set; }
        public T? Value { get; set; }
    }
}