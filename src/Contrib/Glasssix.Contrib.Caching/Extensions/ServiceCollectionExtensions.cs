using Glasssix.Contrib.Caching.Options;
using Glasssix.Contrib.Caching.TypeAlias.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Glasssix.Contrib.Caching.Extensions
{
    /// <summary>
    /// �����̬����ע��
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// �ֲ�ʽ����ע��
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">��������</param>
        /// <param name="typeAliasOptionsAction">��������</param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedCache(this IServiceCollection services,
            Action<DistributedCacheOptions> action,
            Action<TypeAliasOptions>? typeAliasOptionsAction = null)
            => services.AddDistributedCache(Microsoft.Extensions.Options.Options.DefaultName, action, typeAliasOptionsAction);

        /// <summary>
        /// �ֲ�ʽ����ע��
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">ʵ������</param>
        /// <param name="action">��������</param>
        /// <param name="typeAliasOptionsAction">��������</param>
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