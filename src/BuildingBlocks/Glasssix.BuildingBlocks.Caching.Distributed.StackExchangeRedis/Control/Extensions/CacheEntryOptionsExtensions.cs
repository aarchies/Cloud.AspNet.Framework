using Glasssix.Contrib.Caching.Options;
using System;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Extensions
{
    public static class CacheEntryOptionsExtensions
    {
        /// <summary>
        /// 获取绝对生命周期
        /// </summary>
        /// <param name="options"></param>
        /// <param name="creationTime"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DateTimeOffset? GetAbsoluteExpiration(this CacheEntryOptions options, DateTimeOffset creationTime)
        {
            if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
                throw new ArgumentOutOfRangeException(
                    nameof(options),
                    options.AbsoluteExpiration.Value,
                    "绝对过期值必须大于当前时间");

            if (options.AbsoluteExpirationRelativeToNow.HasValue)
                return creationTime.Add(options.AbsoluteExpirationRelativeToNow.Value);

            return options.AbsoluteExpiration;
        }
    }
}