using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using Glasssix.Contrib.Caching.ClientFactory.Distributed;
using Glasssix.Contrib.Caching.ClientFactory.Multilevel;
using Glasssix.Contrib.Caching.Default;
using Glasssix.Contrib.Caching.TypeAlias;
using Glasssix.Contrib.Caching.TypeAlias.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Glasssix.Contrib.Caching.Extensions
{
    public static class CacheDependencyInjection
    {
        /// <summary>
        /// 分布式缓存注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">实例名称</param>
        /// <returns></returns>
        public static IServiceCollection TryAddDistributedCacheCore(IServiceCollection services, string name)
        {
            GlasssixIocApp.TrySetServiceCollection(services);
            services.TryAddSingleton<IDistributedCacheClientFactory, DistributedCacheClientFactoryBase>();
            services.TryAddSingleton(serviceProvider => serviceProvider.GetRequiredService<IDistributedCacheClientFactory>().Create());
            services.TryAddSingleton<ITypeAliasFactory, DefaultTypeAliasFactory>();
            services.Configure<TypeAliasFactoryOptions>(options => options.TryAdd(name));
            return services;
        }

        /// <summary>
        /// 多级缓存注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">实例名称</param>
        /// <returns></returns>
        public static IServiceCollection TryAddMultilevelCacheCore(IServiceCollection services, string name)
        {
            GlasssixIocApp.TrySetServiceCollection(services);
            services.TryAddSingleton<IMultilevelCacheClientFactory, MultilevelCacheClientFactoryBase>();
            services.TryAddSingleton(serviceProvider => serviceProvider.GetRequiredService<IMultilevelCacheClientFactory>().Create());
            services.TryAddSingleton<ITypeAliasFactory, DefaultTypeAliasFactory>();
            services.Configure<TypeAliasFactoryOptions>(options => options.TryAdd(name));
            TryAddDistributedCacheCore(services, name);
            return services;
        }
    }
}