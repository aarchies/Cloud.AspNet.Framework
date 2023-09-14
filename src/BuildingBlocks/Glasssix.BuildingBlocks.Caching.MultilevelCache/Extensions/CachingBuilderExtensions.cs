using Glasssix.BuildingBlocks.Caching.MultilevelCache.Internal;
using Glasssix.BuildingBlocks.Caching.MultilevelCache.Options;
using Glasssix.Contrib.Caching.ClientFactory;
using System;

namespace Glasssix.BuildingBlocks.Caching.MultilevelCache.Extensions
{
    public static class CachingBuilderExtensions
    {
        /// <summary>
        /// ��Ӷ༶����
        /// </summary>
        /// <param name="cachingBuilder"></param>
        /// <param name="sectionName">MultilevelCache�ڵ����ƣ�����Ҫ��Ĭ��ֵ��MultilevelCache��ʹ�ñ������ã�</param>
        /// <param name="isReset">���ø��ĺ��Ƿ�����MemoryCache</param>
        /// <returns></returns>
        public static ICachingBuilder AddMultilevelCache(
            this ICachingBuilder cachingBuilder,
            string sectionName = Constant.DEFAULT_SECTION_NAME,
            bool isReset = false)
        {
            cachingBuilder.Services.AddMultilevelCache(cachingBuilder.Name, sectionName, isReset);
            return cachingBuilder;
        }

        public static ICachingBuilder AddMultilevelCache(this ICachingBuilder cachingBuilder, Action<MultilevelCacheGlobalOptions> action)
        {
            var multilevelCacheOptions = new MultilevelCacheGlobalOptions();
            action.Invoke(multilevelCacheOptions);
            return cachingBuilder.AddMultilevelCache(multilevelCacheOptions);
        }

        public static ICachingBuilder AddMultilevelCache(this ICachingBuilder cachingBuilder, MultilevelCacheGlobalOptions multilevelCacheOptions)
        {
            cachingBuilder.Services.AddMultilevelCache(cachingBuilder.Name, multilevelCacheOptions);
            return cachingBuilder;
        }
    }
}