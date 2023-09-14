using BloomFilter;
using BloomFilter.Redis;
using StackExchange.Redis;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Extensions.Filter
{
    /// <summary>
    /// 基于Redis分布式Bloom
    /// </summary>
    public class RedisBloomFilter
    {
        public readonly IBloomFilter bloom;

        public RedisBloomFilter(ConnectionMultiplexer connection)
        {
            bloom = FilterRedisBuilder.Build(connection, "BoolmFiterArray", 5000000, 0.001);
        }
    }
}