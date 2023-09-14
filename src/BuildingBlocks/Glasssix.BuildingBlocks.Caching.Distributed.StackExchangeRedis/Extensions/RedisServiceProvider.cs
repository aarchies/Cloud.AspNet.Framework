using FireflySoft.RateLimit.AspNetCore;
using FireflySoft.RateLimit.Core.RedisAlgorithm;
using FireflySoft.RateLimit.Core.Rule;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Default;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Extensions.Filter;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.RateLimits;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Extensions
{
    public static class RedisServiceProvider
    {
        /// <summary>
        /// 限流
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <param name="redisContion"></param>
        /// <returns></returns>
        public static IServiceCollection AddRateLimits(this IServiceCollection services, Action<RedisTokenBucketAlgorithmOptions> action, string redisContion)
        {
            var option = new RedisTokenBucketAlgorithmOptions();
            action.Invoke(option);
            services.AddRateLimit(new RedisTokenBucketAlgorithm(
                new[]
                {
                    new TokenBucketRule(option.Capacity,
                        option.InflowQuantityPerUnit,
                        TimeSpan.FromSeconds(option.InflowUnit))
                    {
                        Name = "default limit rule",
                        ExtractTarget = context =>
                        {
                            HttpContext httpContext = (HttpContext) context;

                            return httpContext.Request.Path.Value;
                        },
                        CheckRuleMatching = context =>
                        {
                            HttpContext httpContext = (HttpContext) context;

                            return true;
                        },
                        LockSeconds = option.LockSeconds
                    }
                },
                RedisSentinelConnection.StackExchangeConnectRedis(redisContion)
            ));

            return services;
        }

        /// <summary>
        /// 启用Redis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="_configuration"></param>
        /// <param name="isBloomFilter">布隆拦截器</param>
        /// <param name="IsSentinel">是否哨兵模式</param>
        public static IServiceCollection AddRedis(this IServiceCollection services, string redisContion, bool isBloomFilter = false, bool IsSentinel = false)
        {
            if (!IsSentinel)
            {
                #region Redis

                services.AddSingleton(option =>
                {
                    return RedisSentinelConnection.StackExchangeConnectRedis(redisContion);
                });
                services.AddSingleton<IRedisClient, RedisClient>();

                #endregion Redis
            }
            else
            {
                #region Redis

                services.AddSingleton(option =>
                {
                    return RedisSentinelConnection.StackExchangeSentinelRedis(redisContion);
                });
                services.AddSingleton<IRedisClient, RedisClient>();

                #endregion Redis
            }

            if (isBloomFilter)
            {
                #region Bloom

                services.AddBloomFilter(setup =>
                {
                    setup.UseRedis(redisContion);
                });
                services.AddSingleton<RedisBloomFilter>();

                #endregion Bloom
            }

            return services;
        }
    }
}