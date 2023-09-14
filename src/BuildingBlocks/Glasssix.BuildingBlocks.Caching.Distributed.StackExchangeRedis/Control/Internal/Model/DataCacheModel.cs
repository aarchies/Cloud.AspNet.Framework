using StackExchange.Redis;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal.Model
{
    internal class DataCacheModel
    {
        public DataCacheModel(string key, long? absoluteExpiration, long? slidingExpiration, RedisValue value)
        {
            Key = key;
            AbsoluteExpiration = absoluteExpiration;
            SlidingExpiration = slidingExpiration;
            Value = value;
        }

        public long? AbsoluteExpiration { get; }
        public string Key { get; }
        public long? SlidingExpiration { get; }

        public RedisValue Value { get; }
    }
}