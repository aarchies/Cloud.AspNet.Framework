using Glasssix.Contrib.Data.Options;
using System;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 缓存关系选项
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class CacheRelationOptions<TService> : GlasssixRelationOptions<TService>
        where TService : class
    {
        public CacheRelationOptions(string name, Func<IServiceProvider, TService> func) : base(name)
        {
            Func = func;
        }
    }
}