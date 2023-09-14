using Glasssix.Contrib.Caching.ClientFactory;
using Glasssix.Contrib.Caching.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caching
{

    /// <summary>
    /// 缓存基础类
    /// </summary>
    public abstract class CacheClientBase : ICacheClient
    {
        /// <inheritdoc/>
        public abstract IEnumerable<T?> GetList<T>(params string[] keys);
        /// <inheritdoc/>

        public abstract Task<IEnumerable<T?>> GetListAsync<T>(params string[] keys);
        /// <inheritdoc/>

        public abstract void Refresh<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public abstract Task RefreshAsync<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public abstract void Remove<T>(string key, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public abstract void Remove<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public abstract Task RemoveAsync<T>(string key, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public abstract Task RemoveAsync<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public virtual void Set<T>(string key, T value, DateTimeOffset? absoluteExpiration, Action<CacheOptions>? action = null)
                                    => Set(key, value, new CacheEntryOptions(absoluteExpiration), action);
        /// <inheritdoc/>

        public virtual void Set<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null)
            => Set(key, value, new CacheEntryOptions(absoluteExpirationRelativeToNow), action);
        /// <inheritdoc/>

        public abstract void Set<T>(string key, T value, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public virtual Task SetAsync<T>(string key, T value, DateTimeOffset? absoluteExpiration, Action<CacheOptions>? action = null)
            => SetAsync(key, value, new CacheEntryOptions(absoluteExpiration), action);
        /// <inheritdoc/>

        public virtual Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null)
            => SetAsync(key, value, new CacheEntryOptions(absoluteExpirationRelativeToNow), action);
        /// <inheritdoc/>

        public abstract Task SetAsync<T>(string key, T value, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);
        /// <inheritdoc/>

        public virtual void SetList<T>(Dictionary<string, T?> keyValues,
            DateTimeOffset? absoluteExpiration,
            Action<CacheOptions>? action = null)
            => SetList(keyValues, new CacheEntryOptions(absoluteExpiration), action);
        /// <inheritdoc/>

        public virtual void SetList<T>(Dictionary<string, T?> keyValues,
            TimeSpan? absoluteExpirationRelativeToNow,
            Action<CacheOptions>? action = null)
            => SetList(keyValues, new CacheEntryOptions(absoluteExpirationRelativeToNow), action);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="options"></param>
        /// <param name="action"></param>
        public abstract void SetList<T>(Dictionary<string, T?> keyValues,
            CacheEntryOptions? options = null,
            Action<CacheOptions>? action = null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual Task SetListAsync<T>(Dictionary<string, T?> keyValues,
            DateTimeOffset? absoluteExpiration,
            Action<CacheOptions>? action = null)
            => SetListAsync(keyValues, new CacheEntryOptions(absoluteExpiration), action);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public virtual Task SetListAsync<T>(Dictionary<string, T?> keyValues,
            TimeSpan? absoluteExpirationRelativeToNow,
            Action<CacheOptions>? action = null)
            => SetListAsync(keyValues, new CacheEntryOptions(absoluteExpirationRelativeToNow), action);

        /// <summary>
        /// 批量写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="options"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public abstract Task SetListAsync<T>(Dictionary<string, T?> keyValues,
            CacheEntryOptions? options = null,
            Action<CacheOptions>? action = null);

        /// <summary>
        /// 获取Keys
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        protected static IEnumerable<string> GetKeys(params string[] keys) => keys;
    }
}