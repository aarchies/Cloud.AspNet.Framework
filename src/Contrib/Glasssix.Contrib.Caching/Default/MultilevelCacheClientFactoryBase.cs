using Glasssix.Contrib.Caching.ClientFactory.Multilevel;
using Glasssix.Contrib.Caching.Options;
using Glasssix.Contrib.Data.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Caching.Default
{
    /// <summary>
    /// 多级缓存工厂选项基础类
    /// </summary>
    public class MultilevelCacheClientFactoryBase : CacheClientFactoryBase<IMultilevelCacheClient>, IMultilevelCacheClientFactory
    {
        private readonly IOptionsMonitor<MultilevelCacheFactoryOptions> _optionsMonitor;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public MultilevelCacheClientFactoryBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<MultilevelCacheFactoryOptions>>();
        }

        /// <summary>
        /// 默认空服务提示
        /// </summary>
        protected override string DefaultServiceNotFoundMessage => "未找到默认MultilevelCache,需要添加服务.AddMultilevelCache()";

        /// <summary>
        /// 工厂注入配置选项
        /// </summary>
        protected override GlasssixFactoryOptions<CacheRelationOptions<IMultilevelCacheClient>> FactoryOptions => _optionsMonitor.CurrentValue;

        /// <summary>
        /// 无启用
        /// </summary>
        protected override string SpecifyServiceNotFoundMessage => "确保已启用MultilevelCache!";
    }
}