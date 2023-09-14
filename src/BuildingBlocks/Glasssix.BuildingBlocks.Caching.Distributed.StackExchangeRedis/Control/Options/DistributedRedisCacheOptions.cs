using Glasssix.Contrib.Caching.Options;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Options
{
    public class DistributedRedisCacheOptions
    {
        public CacheEntryOptions? CacheEntryOptions { get; set; }
        public RedisConfigurationOptions? Options { get; set; }
    }
}