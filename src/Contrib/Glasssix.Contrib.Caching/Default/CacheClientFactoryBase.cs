using Glasssix.Contrib.Caching.ClientFactory;
using Glasssix.Contrib.Caching.Options;
using Glasssix.Contrib.Data;
using System;

namespace Glasssix.Contrib.Caching.Default
{
    public abstract class CacheClientFactoryBase<TService> : GlasssixFactoryBase<TService, CacheRelationOptions<TService>>,
        ICacheClientFactory<TService> where TService : class
    {
        protected CacheClientFactoryBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}