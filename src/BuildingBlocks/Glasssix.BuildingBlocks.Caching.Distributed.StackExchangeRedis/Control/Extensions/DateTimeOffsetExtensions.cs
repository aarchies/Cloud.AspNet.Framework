using System;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// 获取过期时间秒数
        /// </summary>
        /// <param name="creationTime">创建时间</param>
        /// <param name="absoluteExpiration">绝对有效期</param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public static long? GetExpirationInSeconds(
            DateTimeOffset creationTime,
            DateTimeOffset? absoluteExpiration,
            TimeSpan? slidingExpiration)
        {
            if (absoluteExpiration.HasValue && slidingExpiration.HasValue)
                return (long)Math.Min(
                    (absoluteExpiration.Value - creationTime).TotalSeconds,
                    slidingExpiration.Value.TotalSeconds);

            if (absoluteExpiration.HasValue)
                return (long)(absoluteExpiration.Value - creationTime).TotalSeconds;

            if (slidingExpiration.HasValue)
                return (long)slidingExpiration.Value.TotalSeconds;

            return null;
        }
    }
}