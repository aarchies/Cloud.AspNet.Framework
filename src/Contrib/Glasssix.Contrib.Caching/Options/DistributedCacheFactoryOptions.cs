using Glasssix.Contrib.Caching.ClientFactory.Distributed;
using Glasssix.Contrib.Data.Options;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 分布式缓存工厂选项
    /// </summary>
    public class DistributedCacheFactoryOptions : GlasssixFactoryOptions<CacheRelationOptions<IDistributedCacheClient>>
    {
    }
}