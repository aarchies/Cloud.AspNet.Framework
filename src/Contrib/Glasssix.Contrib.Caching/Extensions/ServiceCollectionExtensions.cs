using Glasssix.Contrib.Caching.Options;
using Glasssix.Contrib.Caching.TypeAlias.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Glasssix.Contrib.Caching.Extensions
{
    /// <summary>
    /// 缓存多态配置注入
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 分布式缓存注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">缓存配置</param>
        /// <param name="typeAliasOptionsAction">别名配置</param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedCache(this IServiceCollection services,
            Action<DistributedCacheOptions> action,
            Action<TypeAliasOptions>? typeAliasOptionsAction = null)
            => services.AddDistributedCache(Microsoft.Extensions.Options.Options.DefaultName, action, typeAliasOptionsAction);

        /// <summary>
        /// 分布式缓存注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">实例名称</param>
        /// <param name="action">缓存配置</param>
        /// <param name="typeAliasOptionsAction">别名配置</param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, string name,
            Action<DistributedCacheOptions> action,
            Action<TypeAliasOptions>? typeAliasOptionsAction = null)
        {
            CacheDependencyInjection.TryAddDistributedCacheCore(services, name);
            DistributedCacheOptions options = new(services, name);
            action.Invoke(options);

            if (typeAliasOptionsAction != null) services.Configure(name, typeAliasOptionsAction);

            return services;
        }
    }
}