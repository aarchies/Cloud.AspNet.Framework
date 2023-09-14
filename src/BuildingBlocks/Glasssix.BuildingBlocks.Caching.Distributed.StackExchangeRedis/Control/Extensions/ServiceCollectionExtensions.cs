using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Options;
using Glasssix.Contrib.Caching.ClientFactory;
using Glasssix.Contrib.Caching.Extensions;
using Glasssix.Contrib.Caching.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static ICachingBuilder AddStackExchangeRedisCache(
            this IServiceCollection services,
            Action<RedisConfigurationOptions> action)
        {
            var redisConfigurationOptions = new RedisConfigurationOptions();
            action.Invoke(redisConfigurationOptions);
            return services.AddStackExchangeRedisCache(
                Microsoft.Extensions.Options.Options.DefaultName,
                redisConfigurationOptions);
        }

        public static ICachingBuilder AddStackExchangeRedisCache(this IServiceCollection services,
            RedisConfigurationOptions redisConfigurationOptions)
            => services.AddStackExchangeRedisCache(
                Microsoft.Extensions.Options.Options.DefaultName,
                redisConfigurationOptions);

        public static ICachingBuilder AddStackExchangeRedisCache(this IServiceCollection services)
            => services.AddStackExchangeRedisCache(Microsoft.Extensions.Options.Options.DefaultName);

        /// <summary>
        ///添加分布式Redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="redisSectionName">redis节点名，不需要，默认值：RedisConfig（使用本地配置）</param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public static ICachingBuilder AddStackExchangeRedisCache(
             this IServiceCollection services,
             string name,
             string redisSectionName = Const.DEFAULT_REDIS_SECTION_NAME,
             JsonSerializerOptions? jsonSerializerOptions = null)
        {
            CacheDependencyInjection.TryAddDistributedCacheCore(services, name);
            new DistributedCacheOptions(services, name).UseStackExchangeRedisCache(redisSectionName, jsonSerializerOptions);
            return new CachingBuilder(services, name);
        }

        public static ICachingBuilder AddStackExchangeRedisCache(
             this IServiceCollection services,
             string name,
             Action<RedisConfigurationOptions> action,
             JsonSerializerOptions? jsonSerializerOptions = null)
        {
            var redisConfigurationOptions = new RedisConfigurationOptions();
            action.Invoke(redisConfigurationOptions);
            return services.AddStackExchangeRedisCache(
                name,
                redisConfigurationOptions,
                jsonSerializerOptions);
        }

        public static ICachingBuilder AddStackExchangeRedisCache(
              this IServiceCollection services,
              string name,
              RedisConfigurationOptions redisConfigurationOptions,
              JsonSerializerOptions? jsonSerializerOptions = null)
        {
            CacheDependencyInjection.TryAddDistributedCacheCore(services, name);

            services.UseStackExchangeRedisCache(name, redisConfigurationOptions, jsonSerializerOptions);

            return new CachingBuilder(services, name);
        }
    }
}