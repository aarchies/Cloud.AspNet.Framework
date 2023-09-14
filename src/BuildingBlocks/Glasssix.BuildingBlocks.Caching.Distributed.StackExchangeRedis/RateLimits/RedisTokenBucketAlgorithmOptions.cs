namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.RateLimits
{
    public class RedisTokenBucketAlgorithmOptions
    {
        /// <summary>
        /// 令牌桶容量
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 单位时间流入量
        /// </summary>
        public int InflowQuantityPerUnit { get; set; }

        /// <summary>
        /// 流入铲斗的时间单位,秒计算
        /// </summary>
        public int InflowUnit { get; set; }

        /// <summary>
        /// 触发速率限制后锁定的秒数。0表示未锁定
        /// </summary>
        public int LockSeconds { get; set; }
    }
}