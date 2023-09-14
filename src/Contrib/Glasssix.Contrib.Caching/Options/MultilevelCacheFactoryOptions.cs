using Glasssix.Contrib.Caching.ClientFactory.Multilevel;
using Glasssix.Contrib.Data.Options;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 多级缓存工厂选项
    /// </summary>
    public class MultilevelCacheFactoryOptions : GlasssixFactoryOptions<CacheRelationOptions<IMultilevelCacheClient>>
    {
    }
}