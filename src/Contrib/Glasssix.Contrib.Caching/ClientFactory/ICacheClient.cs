using Glasssix.Contrib.Caching.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caching.ClientFactory
{
    public interface ICacheClient
    {
        IEnumerable<T?> GetList<T>(params string[] keys);

        Task<IEnumerable<T?>> GetListAsync<T>(params string[] keys);

        /// <summary>
        /// 刷新缓存生命周期
        /// </summary>
        /// <param name="keys">缓存键的集合，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        void Refresh<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);

        /// <summary>
        /// 刷新缓存生命周期
        /// </summary>
        /// <param name="keys">缓存键的集合，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        Task RefreshAsync<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);

        /// <summary>
        /// delete cache key
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void Remove<T>(string key, Action<CacheOptions>? action = null);

        /// <summary>
        /// delete cache key set
        /// </summary>
        /// <param name="keys">缓存键的集合，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void Remove<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);

        /// <summary>
        /// delete cache key
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task RemoveAsync<T>(string key, Action<CacheOptions>? action = null);

        /// <summary>
        /// delete cache key set
        /// </summary>
        /// <param name="keys">缓存键的集合，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task RemoveAsync<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="value">Cache value</param>
        /// <param name="absoluteExpiration">绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string key, T value, DateTimeOffset? absoluteExpiration, Action<CacheOptions>? action = null);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="value">Cache value</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="value">Cache value</param>
        /// <param name="options">配置缓存生命周期，该生命周期在为空时与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string key, T value, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="value">Cache value</param>
        /// <param name="absoluteExpiration">绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        Task SetAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration, Action<CacheOptions>? action = null);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="value">Cache value</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="value">Cache value</param>
        /// <param name="options">配置缓存生命周期，该生命周期在为空时与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        Task SetAsync<T>(string key, T value, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="keyValues">键值对的集合</param>
        /// <param name="absoluteExpiration">绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void SetList<T>(Dictionary<string, T?> keyValues, DateTimeOffset? absoluteExpiration, Action<CacheOptions>? action = null);

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="keyValues">键值对的集合</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void SetList<T>(Dictionary<string, T?> keyValues, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null);

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="keyValues">键值对的集合</param>
        /// <param name="options">配置缓存生命周期，该生命周期在为空时与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        void SetList<T>(Dictionary<string, T?> keyValues, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="keyValues">键值对的集合</param>
        /// <param name="absoluteExpiration">绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        Task SetListAsync<T>(Dictionary<string, T?> keyValues, DateTimeOffset? absoluteExpiration, Action<CacheOptions>? action = null);

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="keyValues">键值对的集合</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期，当为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        Task SetListAsync<T>(Dictionary<string, T?> keyValues, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null);

        /// <summary>
        ///批量设置缓存
        /// </summary>
        /// <param name="keyValues">键值对的集合</param>
        /// <param name="options">配置缓存生命周期，该生命周期在为空时与默认配置一致</param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        Task SetListAsync<T>(Dictionary<string, T?> keyValues, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);
    }
}