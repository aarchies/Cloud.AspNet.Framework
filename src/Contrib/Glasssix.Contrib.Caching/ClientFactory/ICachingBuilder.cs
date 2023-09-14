using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.Contrib.Caching.ClientFactory
{
    public interface ICachingBuilder
    {
        /// <summary>
        ///获取用于多个IDistributedCacheClient或IMultilableCacheClient 实例名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 应用服务集合
        /// </summary>
        IServiceCollection Services { get; }
    }
}