using Glasssix.Contrib.Caching.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caching.ClientFactory.Distributed
{
    public interface IDistributedCacheClient : ICacheClient
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
        ///获取缓存密钥是否存在
        /// </summary>
        /// <param name="key">完整的缓存密钥，缓存密钥不再格式化</param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        ///获取缓存密钥是否存在
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Exists<T>(string key, Action<CacheOptions>? action = null);

        /// <summary>
        ///获取缓存密钥是否存在
        /// </summary>
        /// <param name="key">完整的缓存密钥，缓存密钥不再格式化</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        ///获取缓存密钥是否存在
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<bool> ExistsAsync<T>(string key, Action<CacheOptions>? action = null);

        T? Get<T>(string key, Action<CacheOptions>? action = null);

        Task<T?> GetAsync<T>(string key, Action<CacheOptions>? action = null);

        IEnumerable<KeyValuePair<string, T?>> GetByKeyPattern<T>(string keyPattern, Action<CacheOptions>? action = null);

        Task<IEnumerable<KeyValuePair<string, T?>>> GetByKeyPatternAsync<T>(string keyPattern, Action<CacheOptions>? action = null);

        /// <summary>
        /// 仅获取密钥，不触发更新过期时间策略
        /// </summary>
        /// <param name="keyPattern">完成key，不再格式化，例如：app_*</param>
        /// <returns></returns>
        IEnumerable<string> GetKeys(string keyPattern);

        /// <summary>
        /// 根据Key模糊匹配匹配规则的Keys
        /// 根据获取的类型和KeyPattern获取满足规则的密钥集
        /// </summary>
        /// <param name="keyPattern">keyPattern，用于更改全局缓存配置信息，例如：1*</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<string> GetKeys<T>(string keyPattern, Action<CacheOptions>? action = null);

        /// <summary>
        /// 仅获取密钥，不触发更新过期时间策略
        /// </summary>
        /// <param name="keyPattern">完成keyPattern，不再格式化，例如：app_*</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetKeysAsync(string keyPattern);

        /// <summary>
        /// 根据密钥模糊匹配匹配规则的缓存密钥集
        /// 根据获取的类型和KeyPattern获取满足规则的密钥集
        /// </summary>
        /// <param name="keyPattern">keyPattern，用于更改全局缓存配置信息，例如：1*</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<string>> GetKeysAsync<T>(string keyPattern, Action<CacheOptions>? action = null);

        IEnumerable<T?> GetList<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);

        Task<IEnumerable<T?>> GetListAsync<T>(IEnumerable<string> keys, Action<CacheOptions>? action = null);

        T? GetOrSet<T>(string key, Func<CacheEntry<T>> setter, Action<CacheOptions>? action = null);

        Task<T?> GetOrSetAsync<T>(string key, Func<CacheEntry<T>> setter, Action<CacheOptions>? action = null);

        Task<T?> GetOrSetAsync<T>(string key, Func<Task<CacheEntry<T>>> setter, Action<CacheOptions>? action = null);

        /// <summary>
        /// 增量哈希
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="value">增量，必须大于0</param>
        /// <param name="defaultMinVal">默认最小值</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <param name="options">配置缓存生命周期，该生命周期在为空时与默认配置一致（仅在配置不存在时初始化）</param>
        /// <returns>失败时返回null，成功时返回减量操作后的字段值</returns>
        Task<long?> HashDecrementAsync(string key,
            long value = 1L,
            long defaultMinVal = 0L,
            Action<CacheOptions>? action = null,
            CacheEntryOptions? options = null);

        /// <summary>
        /// 增量哈希
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="value">增量，必须大于0</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <param name="options">配置缓存生命周期，该生命周期在为空时与默认配置一致（仅在配置不存在时初始化）</param>
        /// <returns>返回增量操作后的字段值</returns>
        Task<long> HashIncrementAsync(string key,
            long value = 1L,
            Action<CacheOptions>? action = null,
            CacheEntryOptions? options = null);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">完整缓存密钥</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期时间，为空时永久有效</param>
        /// <returns></returns>
        bool KeyExpire(string key, TimeSpan? absoluteExpirationRelativeToNow);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期时间，为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <returns></returns>
        bool KeyExpire<T>(string key, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">完整缓存密钥</param>
        /// <param name="absoluteExpiration">绝对过期，空时永久有效</param>
        /// <returns></returns>
        bool KeyExpire(string key, DateTimeOffset absoluteExpiration);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="absoluteExpiration">绝对过期，空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <returns></returns>
        bool KeyExpire<T>(string key, DateTimeOffset absoluteExpiration, Action<CacheOptions>? action = null);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">完整缓存密钥</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <returns></returns>
        bool KeyExpire(string key, CacheEntryOptions? options = null);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <returns></returns>
        bool KeyExpire<T>(string key, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);

        /// <summary>
        /// 刷新缓存密钥集的生命周期
        /// </summary>
        /// <param name="keys">缓存密钥的集合</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <returns>获取已成功刷新缓存密钥生命周期的缓存数</returns>
        long KeyExpire(IEnumerable<string> keys, CacheEntryOptions? options = null);

        /// <summary>
        /// 刷新缓存密钥集的生命周期
        /// </summary>
        /// <param name="keys">缓存键的集合，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>获取已成功刷新缓存密钥生命周期的缓存数</returns>
        long KeyExpire<T>(IEnumerable<string> keys, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">完整缓存密钥</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期时间，为空时永久有效</param>
        /// <returns></returns>
        Task<bool> KeyExpireAsync(string key, TimeSpan? absoluteExpirationRelativeToNow);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="absoluteExpirationRelativeToNow">相对于现在的绝对过期时间，为空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <returns></returns>
        Task<bool> KeyExpireAsync<T>(string key, TimeSpan? absoluteExpirationRelativeToNow, Action<CacheOptions>? action = null);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">完整缓存密钥</param>
        /// <param name="absoluteExpiration">绝对过期，空时永久有效</param>
        /// <returns></returns>
        Task<bool> KeyExpireAsync(string key, DateTimeOffset absoluteExpiration);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="absoluteExpiration">绝对过期，空时永久有效</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <returns></returns>
        Task<bool> KeyExpireAsync<T>(string key, DateTimeOffset absoluteExpiration, Action<CacheOptions>? action = null);

        /// <summary>
        ///设置缓存声明周期
        /// </summary>
        /// <param name="key">完整缓存密钥</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <returns></returns>
        Task<bool> KeyExpireAsync(string key, CacheEntryOptions? options = null);

        /// <summary>
        ///设置缓存生命周期
        /// </summary>
        /// <param name="key">缓存键，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <returns></returns>
        Task<bool> KeyExpireAsync<T>(string key, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);

        /// <summary>
        ///批量设置缓存生命周期
        /// </summary>
        /// <param name="keys">缓存密钥的集合</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <returns></returns>
        Task<long> KeyExpireAsync(IEnumerable<string> keys, CacheEntryOptions? options = null);

        /// <summary>
        /// 批量设置缓存生命周期
        /// </summary>
        /// <param name="keys">缓存键的集合，实际缓存键将根据全局配置和操作决定是否格式化缓存键</param>
        /// <param name="options">配置缓存生命周期，当其为空时，该生命周期与默认配置一致</param>
        /// <param name="action">缓存配置，用于更改全局缓存配置信息</param>
        /// <returns></returns>
        Task<long> KeyExpireAsync<T>(IEnumerable<string> keys, CacheEntryOptions? options = null, Action<CacheOptions>? action = null);

        /// <summary>
        /// 跳过式调用锁，如果事情正在被调用，直接跳过
        /// </summary>
        /// <param name="resource">锁定资源的标识</param>
        /// <param name="expiryTime">锁过期时间</param>
        /// <param name="work"></param>
        /// <returns></returns>
        Task<bool> OverlappingWork([NotNull] string resource, TimeSpan expiryTime, Func<Task> work);

        void Publish(string channel, Action<PublishOptions> options);

        Task PublishAsync(string channel, Action<PublishOptions> options);

        /// <summary>
        /// 刷新缓存生命周期
        /// </summary>
        /// <param name="keys"></param>
        void Refresh(params string[] keys);

        /// <summary>
        /// 刷新缓存生命周期
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task RefreshAsync(params string[] keys);

        /// <summary>
        /// 刷新缓存生命周期
        /// </summary>
        /// <param name="keys">缓存密钥的集合</param>
        void Remove(params string[] keys);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys">缓存密钥的集合</param>
        /// <returns></returns>
        Task RemoveAsync(params string[] keys);

        #region Sort

        /// <summary>
        /// 添加排序Hash
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        Task<bool> SortedSetAddAsync(string redisKey, string redisValue, double score);

        /// <summary>
        /// 获取排序Hash指定field value
        /// </summary>
        /// <param name="redisKey">key</param>
        /// <param name="redisValue">field</param>
        /// <returns>score</returns>
        Task<double?> SortedSetScoreAsync(string redisKey, string redisValue);

        Task<bool> SortedSetRemoveAsync(string redisKey, string redisValue);

        Task<List<T?>?> SortedSetRangeByScoreAsync<T>(string redisKey, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, int order = 0);

        /// <summary>
        /// 获取排序hash长度
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<long> SortedSetLengthAsync(string redisKey);

        /// <summary>
        /// 删除范围内hash值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        Task<long> SortedSetRemoveRangeByScoreAsync(string redisKey, double start, double stop);

        #endregion Sort

        void Subscribe<T>(string channel, Action<SubscribeOptions<T>> options);

        Task SubscribeAsync<T>(string channel, Action<SubscribeOptions<T>> options);

        void UnSubscribe<T>(string channel);

        Task UnSubscribeAsync<T>(string channel);
    }
}