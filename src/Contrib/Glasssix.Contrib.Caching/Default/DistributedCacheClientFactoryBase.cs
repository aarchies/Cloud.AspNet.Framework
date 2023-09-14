using Glasssix.Contrib.Caching.ClientFactory.Distributed;
using Glasssix.Contrib.Caching.Options;
using Glasssix.Contrib.Data.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Caching.Default
{
    public class DistributedCacheClientFactoryBase : CacheClientFactoryBase<IDistributedCacheClient>, IDistributedCacheClientFactory
    {
        private readonly IOptionsMonitor<DistributedCacheFactoryOptions> _optionsMonitor;

        public DistributedCacheClientFactoryBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<DistributedCacheFactoryOptions>>();
        }

        protected override string DefaultServiceNotFoundMessage => "未找到默认DistributedCache， 需要添加服务.AddStackExchangeRedisCache()";

        protected override GlasssixFactoryOptions<CacheRelationOptions<IDistributedCacheClient>> FactoryOptions => _optionsMonitor.CurrentValue;
        protected override string SpecifyServiceNotFoundMessage => "确保已启用DistributedCache!";
    }
}