using Glasssix.BuildingBlocks.Caching.MultilevelCache.Internal;
using Glasssix.BuildingBlocks.Caching.MultilevelCache.Internal.Options;
using Glasssix.BuildingBlocks.Caching.MultilevelCache.Options;
using Glasssix.BuildingBlocks.Configuration.Extensions;
using Glasssix.Contrib.Caching.ClientFactory.Distributed;
using Glasssix.Contrib.Caching.ClientFactory.Multilevel;
using Glasssix.Contrib.Caching.Extensions;
using Glasssix.Contrib.Caching.Options;
using Glasssix.Contrib.Caching.TypeAlias;
using Glasssix.Contrib.Caching.TypeAlias.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Glasssix.BuildingBlocks.Caching.MultilevelCache.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 多级缓存注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="distributedCacheAction">分布式缓存配置</param>
        /// <param name="multilevelCacheOptionsAction">多级缓存配置</param>
        /// <param name="typeAliasOptionsAction">别名配置</param>
        /// <returns></returns>
        public static IServiceCollection AddMultilevelCache(this IServiceCollection services, Action<DistributedCacheOptions> distributedCacheAction, Action<MultilevelCacheGlobalOptions>? multilevelCacheOptionsAction = null, Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            if (multilevelCacheOptionsAction == null)
                return services.AddMultilevelCache(distributedCacheAction, Constant.DEFAULT_SECTION_NAME, typeAliasOptionsAction);

            return services.AddMultilevelCache(Microsoft.Extensions.Options.Options.DefaultName, distributedCacheAction, multilevelCacheOptionsAction, typeAliasOptionsAction);
        }

        public static IServiceCollection AddMultilevelCache(this IServiceCollection services, Action<DistributedCacheOptions> distributedCacheAction, string sectionName, Action<TypeAliasOptions>? typeAliasOptionsAction)
            => services.AddMultilevelCache(distributedCacheAction, sectionName, false, typeAliasOptionsAction);

        public static IServiceCollection AddMultilevelCache(this IServiceCollection services, Action<DistributedCacheOptions> distributedCacheAction, string sectionName, bool isReset = false, Action<TypeAliasOptions>? typeAliasOptionsAction = null)
            => services.AddMultilevelCache(Microsoft.Extensions.Options.Options.DefaultName, distributedCacheAction, sectionName, isReset, typeAliasOptionsAction);

        public static IServiceCollection AddMultilevelCache(this IServiceCollection services, string name, Action<DistributedCacheOptions> distributedCacheAction, string sectionName, bool isReset = false, Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            services.AddMultilevelCache(name, sectionName, isReset, typeAliasOptionsAction);
            var distributedCacheOptions = new DistributedCacheOptions(services, name);
            distributedCacheAction.Invoke(distributedCacheOptions);
            return services;
        }

        public static IServiceCollection AddMultilevelCache(
            this IServiceCollection services,
            string name,
            Action<DistributedCacheOptions> distributedCacheAction,
            Action<MultilevelCacheGlobalOptions>? multilevelCacheOptionsAction = null,
            Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            if (multilevelCacheOptionsAction == null)
                return services.AddMultilevelCache(name, distributedCacheAction, Constant.DEFAULT_SECTION_NAME, typeAliasOptionsAction: typeAliasOptionsAction);

            MultilevelCacheGlobalOptions multilevelCacheGlobalOptions = new();
            multilevelCacheOptionsAction(multilevelCacheGlobalOptions);
            services.AddMultilevelCache(name, multilevelCacheGlobalOptions, typeAliasOptionsAction);
            var distributedCacheOptions = new DistributedCacheOptions(services, name);
            distributedCacheAction.Invoke(distributedCacheOptions);

            return services;
        }

        public static IServiceCollection AddMultilevelCache(
            this IServiceCollection services,
            string name,
            Action<DistributedCacheOptions> distributedCacheAction,
            IConfiguration configuration,
            bool isReset = false,
            Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            services.AddMultilevelCache(name, configuration, isReset, typeAliasOptionsAction);
            var distributedCacheOptions = new DistributedCacheOptions(services, name);
            distributedCacheAction.Invoke(distributedCacheOptions);
            return services;
        }

        #region internal methods

        internal static void AddMultilevelCache(
            this IServiceCollection services,
            string name,
            MultilevelCacheGlobalOptions multilevelCacheGlobalOptions,
            Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            CacheDependencyInjection.TryAddMultilevelCacheCore(services, name);

            services.Configure<MultilevelCacheFactoryOptions>(options =>
            {
                if (options.Options.Any(opt => opt.Name == name))
                    return;

                var cacheRelationOptions = new CacheRelationOptions<IMultilevelCacheClient>(name, serviceProvider =>
                {
                    var distributedCacheClientFactory = serviceProvider.GetRequiredService<IDistributedCacheClientFactory>();
                    var multilevelCacheClient = new MultilevelCacheClient(
                        new MemoryCache(multilevelCacheGlobalOptions),
                        distributedCacheClientFactory.Create(name),
                        new MultilevelCacheOptions
                        {
                            CacheKeyType = multilevelCacheGlobalOptions.GlobalCacheOptions.CacheKeyType,
                            MemoryCacheEntryOptions = multilevelCacheGlobalOptions.CacheEntryOptions
                        },
                        multilevelCacheGlobalOptions.SubscribeKeyType,
                        multilevelCacheGlobalOptions.SubscribeKeyPrefix,
                        serviceProvider.GetRequiredService<ITypeAliasFactory>().Create(name)
                    );
                    return multilevelCacheClient;
                });
                options.Options.Add(cacheRelationOptions);
            });

            if (typeAliasOptionsAction != null)
                services.Configure(name, typeAliasOptionsAction);
        }

        internal static void AddMultilevelCache(
            this IServiceCollection services,
            string name,
            string sectionName,
            bool isReset = false,
            Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            services.AddConfigure<MultilevelCacheGlobalOptions>(sectionName, name);

           services.AddMultilevelCache(name, isReset, typeAliasOptionsAction);
        }

        private static void AddMultilevelCache(this IServiceCollection services, string name, IConfiguration configuration, bool isReset = false, Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            services.Configure<MultilevelCacheGlobalOptions>(name, configuration);
            services.AddMultilevelCache(name, isReset, typeAliasOptionsAction);
        }

        private static void AddMultilevelCache(this IServiceCollection services, string name, bool isReset = false, Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            CacheDependencyInjection.TryAddMultilevelCacheCore(services, name);

            services.Configure<MultilevelCacheFactoryOptions>(options =>
            {
                if (options.Options.Any(opt => opt.Name == name))
                    return;

                var cacheRelationOptions = new CacheRelationOptions<IMultilevelCacheClient>(name, serviceProvider =>
                {
                    var distributedCacheClientFactory = serviceProvider.GetRequiredService<IDistributedCacheClientFactory>();
                    var multilevelCacheClient = new MultilevelCacheClient(
                        name,
                        isReset,
                        serviceProvider.GetRequiredService<IOptionsMonitor<MultilevelCacheGlobalOptions>>(),
                        distributedCacheClientFactory.Create(name),
                        serviceProvider.GetRequiredService<ITypeAliasFactory>().Create(name)
                    );
                    return multilevelCacheClient;
                });
                options.Options.Add(cacheRelationOptions);
            });

            if (typeAliasOptionsAction != null)
                services.Configure(name, typeAliasOptionsAction);
        }

        #endregion internal methods
    }
}