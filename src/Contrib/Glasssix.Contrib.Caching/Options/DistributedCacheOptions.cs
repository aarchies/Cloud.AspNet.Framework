using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 分布式缓存选项
    /// </summary>
    public class DistributedCacheOptions
    {
        public DistributedCacheOptions(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        /// <summary>
        /// Gets the name of the client configured by this builder.
        /// </summary>
        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}