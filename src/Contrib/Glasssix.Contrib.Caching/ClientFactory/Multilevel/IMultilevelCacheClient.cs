using Glasssix.Contrib.Caching.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caching.ClientFactory.Multilevel
{
    public interface IMultilevelCacheClient : ICacheClient
    {
        /// <summary>
        ///  阻塞式调用，事情最终会被调用（等待时间内）
        /// </summary>
        /// <param name="resource">锁定资源的标识</param>
        /// <param name="expiryTime">锁过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <param name="work"></param>
        /// <returns></returns>
        Task<bool> BlockingWork([NotNull] string resource, TimeSpan expiryTime, TimeSpan waitTime, Func<Task> work);

        /// <summary>
        /// Get cache
        /// 当内存缓存不存在时，获取分布式缓存的结果并将结果存储在内存缓存中（内存缓存的有效期是传入的过期时间）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T? Get<T>(string key, Action<MultilevelCacheOptions>? action = null);

        /// <summary>
        /// Get cache
        /// 当内存缓存不存在时，获取分布式缓存的结果并将结果存储在内存缓存中（内存缓存的有效期是传入的过期时间）
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="valueChanged"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T? Get<T>(string key, Action<T?> valueChanged, Action<MultilevelCacheOptions>? action = null);

        /// <summary>
        /// Get cache
        /// 当内存缓存不存在时，获取分布式缓存的结果并将结果存储在内存缓存中（内存缓存的有效期是传入的过期时间）
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T?> GetAsync<T>(string key, Action<MultilevelCacheOptions>? action = null);

        /// <summary>
        /// Get cache
        /// 当内存缓存不存在时，获取分布式缓存的结果并将结果存储在内存缓存中（内存缓存的有效期是传入的过期时间）
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="valueChanged"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T?> GetAsync<T>(string key, Action<T?> valueChanged, Action<MultilevelCacheOptions>? action = null);

        /// <summary>
        /// Get cache collection
        /// 当内存缓存不存在时，获取分布式缓存的结果并将结果存储在内存缓存中（内存缓存的有效期是传入的过期时间）
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T?> GetList<T>(IEnumerable<string> keys, Action<MultilevelCacheOptions>? action = null);

        /// <summary>
        /// Get cache collection
        /// 当内存缓存不存在时，获取分布式缓存的结果并将结果存储在内存缓存中（内存缓存的有效期是传入的过期时间）
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<T?>> GetListAsync<T>(IEnumerable<string> keys, Action<MultilevelCacheOptions>? action = null);

        /// <summary>
        /// 获取缓存，如果缓存不存在，则设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="distributedCacheEntryFunc">内存缓存不存在时返回的分布式缓存信息</param>
        /// <param name="memoryCacheEntryOptions">内存缓存生存期配置，在为空时与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T? GetOrSet<T>(string key,
            Func<CacheEntry<T>> distributedCacheEntryFunc,
            CacheEntryOptions? memoryCacheEntryOptions = null,
            Action<CacheOptions>? action = null);

        /// <summary>
        /// 获取缓存，如果缓存不存在，则设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="combinedCacheEntry">缓存密钥信息，用于在缓存不存在时配置处理器的执行，以及内存缓存生命周期</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T? GetOrSet<T>(string key, CombinedCacheEntry<T> combinedCacheEntry, Action<CacheOptions>? action = null);

        /// <summary>
        /// 获取缓存，如果缓存不存在，则设置缓存
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="distributedCacheEntryFunc">内存缓存不存在时返回的分布式缓存信息</param>
        /// <param name="memoryCacheEntryOptions">内存缓存生存期配置，在为空时与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T?> GetOrSetAsync<T>(
            string key,
            Func<CacheEntry<T>> distributedCacheEntryFunc,
            CacheEntryOptions? memoryCacheEntryOptions = null,
            Action<CacheOptions>? action = null);

        /// <summary>
        /// Get cache, set cache if cache does not exist
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="distributedCacheEntryFunc">内存缓存不存在时返回的分布式缓存信息</param>
        /// <param name="memoryCacheEntryOptions">内存缓存生存期配置，在为空时与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T?> GetOrSetAsync<T>(
            string key,
            Func<Task<CacheEntry<T>>> distributedCacheEntryFunc,
            CacheEntryOptions? memoryCacheEntryOptions = null,
            Action<CacheOptions>? action = null);

        Task<T?> GetOrSetAsync<T>(string key, CombinedCacheEntry<T> combinedCacheEntry, Action<CacheOptions>? action = null);

        /// <summary>
        /// 跳过式调用锁，如果事情正在被调用，直接跳过
        /// </summary>
        /// <param name="resource">锁定资源的标识</param>
        /// <param name="expiryTime">锁过期时间</param>
        /// <param name="work"></param>
        /// <returns></returns>
        Task<bool> OverlappingWork([NotNull] string resource, TimeSpan expiryTime, Func<Task> work);

        /// <summary>
        /// 刷新缓存生命周期
        /// </summary>
        /// <param name="keys">Set of cache keys</param>
        void Refresh<T>(params string[] keys);

        /// <summary>
        /// 刷新缓存生命周期
        /// </summary>
        /// <param name="keys">Set of cache keys</param>
        Task RefreshAsync<T>(params string[] keys);

        void Remove<T>(params string[] keys);

        Task RemoveAsync<T>(params string[] keys);

        void Set<T>(string key,
            T value,
            CacheEntryOptions? distributedOptions,
            CacheEntryOptions? memoryOptions,
            Action<CacheOptions>? action = null);

        void Set<T>(string key, T value, CombinedCacheEntryOptions? options, Action<CacheOptions>? action = null);

        Task SetAsync<T>(
            string key,
            T value,
            CacheEntryOptions? distributedOptions,
            CacheEntryOptions? memoryOptions,
            Action<CacheOptions>? action = null);

        Task SetAsync<T>(string key, T value, CombinedCacheEntryOptions? options, Action<CacheOptions>? action = null);

        void SetList<T>(
            Dictionary<string, T?> keyValues,
            CacheEntryOptions? distributedOptions,
            CacheEntryOptions? memoryOptions,
            Action<CacheOptions>? action = null);

        void SetList<T>(Dictionary<string, T?> keyValues, CombinedCacheEntryOptions? options, Action<CacheOptions>? action = null);

        Task SetListAsync<T>(
            Dictionary<string, T?> keyValues,
            CacheEntryOptions? distributedOptions,
            CacheEntryOptions? memoryOptions,
            Action<CacheOptions>? action = null);

        Task SetListAsync<T>(Dictionary<string, T?> keyValues, CombinedCacheEntryOptions? options, Action<CacheOptions>? action = null);
    }
}