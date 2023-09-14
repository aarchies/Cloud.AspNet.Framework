using Glasssix.Contrib.Caching.Options;
using System;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caching
{
    public class CombinedCacheEntry<T>
    {
        public CombinedCacheEntry()
        {
        }

        public CombinedCacheEntry(Func<CacheEntry<T>> distributedCacheEntryFunc, CacheEntryOptions? memoryCacheEntryOptions)
            : this(memoryCacheEntryOptions)
        {
            DistributedCacheEntryFunc = distributedCacheEntryFunc;
        }

        public CombinedCacheEntry(Func<Task<CacheEntry<T>>> distributedCacheEntryAsyncFunc, CacheEntryOptions? memoryCacheEntryOptions)
            : this(memoryCacheEntryOptions)
        {
            DistributedCacheEntryAsyncFunc = distributedCacheEntryAsyncFunc;
        }

        private CombinedCacheEntry(CacheEntryOptions? memoryCacheEntryOptions) : this()
        {
            MemoryCacheEntryOptions = memoryCacheEntryOptions;
        }

        public Func<Task<CacheEntry<T>>>? DistributedCacheEntryAsyncFunc { get; set; }
        public Func<CacheEntry<T>>? DistributedCacheEntryFunc { get; set; }
        public CacheEntryOptions? MemoryCacheEntryOptions { get; set; }
    }
}