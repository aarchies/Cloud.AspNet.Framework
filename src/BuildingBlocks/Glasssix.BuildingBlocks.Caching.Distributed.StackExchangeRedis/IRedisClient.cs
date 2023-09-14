using BloomFilter;
using StackExchange.Redis;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis
{
    public interface IRedisClient
    {
        /// <summary>
        /// 过滤器
        /// </summary>
        IBloomFilter bloomFilter { get; }

        /// <summary>
        /// 连接
        /// </summary>
        ConnectionMultiplexer connection { get; }

        /// <summary>
        /// DB
        /// </summary>
        IDatabase Database { get; }

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
        ///  删除Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task DeleteKeyAsync(string redisKey);

        /// <summary>
        ///  Key续期
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        Task<bool> ExpireKeyAsync(string redisKey, TimeSpan time);

        /// <summary>
        /// Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ValueTask<bool> KeyExistsAsync(string key);

        /// <summary>
        /// 获取一个对象(会进行反序列化)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        T ObjectGet<T>(string redisKey, TimeSpan? expiry = null);

        /// <summary>
        /// 异步获取一个对象（会进行反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<T> ObjectGetAsync<T>(string redisKey, TimeSpan? expiry = null);

        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool ObjectSet<T>(string redisKey, T redisValue, TimeSpan? expiry = null);

        /// <summary>
        /// 异步存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> ObjectSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null);

        /// <summary>
        /// 跳过式调用锁，如果事情正在被调用，直接跳过
        /// </summary>
        /// <param name="resource">锁定资源的标识</param>
        /// <param name="expiryTime">锁过期时间</param>
        /// <param name="work"></param>
        /// <returns></returns>
        Task<bool> OverlappingWork([NotNull] string resource, TimeSpan expiryTime, Func<Task> work);

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        string StringGet(string redisKey, TimeSpan? expiry = null);

        /// <summary>
        /// 异步设置 key 并保存字符串（如果 key 已存在，则覆盖值）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<string> StringGetAsync(string redisKey, TimeSpan? expiry = null);

        /// <summary>
        /// 根据当前key进行Value追加
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        Task<long> StringIncrementAsync(string redisKey);

        /// <summary>
        /// 设置 key 并保存字符串（如果 key 已存在，则覆盖值）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null);

        /// <summary>
        /// 异步获取字符串
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null);
    }
}