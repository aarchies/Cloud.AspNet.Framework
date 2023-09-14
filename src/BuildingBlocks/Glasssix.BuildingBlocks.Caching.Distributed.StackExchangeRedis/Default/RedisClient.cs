using BloomFilter;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Extensions.Filter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Default
{
    public class RedisClient : IRedisClient
    {
        public readonly ILogger<RedisClient> _logger;
        private readonly IBloomFilter _bloomFilter;
        private readonly List<RedLockMultiplexer> redLockMultiplexers;
        private ConnectionMultiplexer _conn;
        private IDatabase _db;

        public RedisClient(ConnectionMultiplexer conn, ILoggerFactory loggerFactory, RedisBloomFilter bloomFilter = null)
        {
            _conn = conn;
            _bloomFilter = bloomFilter?.bloom;
            _logger = loggerFactory.CreateLogger<RedisClient>();
            _db = conn.GetDatabase();
            redLockMultiplexers = new List<RedLockMultiplexer> { conn };
        }

        public IBloomFilter bloomFilter => _bloomFilter;
        public ConnectionMultiplexer connection => _conn;
        public IDatabase Database => _db;

        public async Task<bool> BlockingWork([NotNull] string resource, TimeSpan expiryTime, TimeSpan waitTime, Func<Task> work)
        {
            resource = CreateKey(resource);
            using (RedLockFactory redisLockFactory = RedLockFactory.Create(redLockMultiplexers))
            {
                using (IRedLock redisLock = await redisLockFactory.CreateLockAsync(resource, expiryTime, waitTime, TimeSpan.FromSeconds(1)))
                {
                    if (redisLock.IsAcquired)
                    {
                        await work.Invoke();

                        return true;
                    }
                }
                return false;
            }
        }

        public async Task DeleteKeyAsync(string redisKey) => await _db.KeyDeleteAsync(redisKey);

        public async Task<bool> ExpireKeyAsync(string redisKey, TimeSpan time) => await _db.KeyExpireAsync(redisKey, time);

        public async ValueTask<bool> KeyExistsAsync(string key) => await _db.KeyExistsAsync(key);

        public T ObjectGet<T>(string redisKey, TimeSpan? expiry = null)
        {
            if (_bloomFilter != null)
                if (!_bloomFilter.Contains(redisKey))
                {
                    //this._logger.LogWarning($"执行CacheGet-Object时,当前Key:{redisKey}不存在!");
                    return default;
                }
            var result = JsonConvert.DeserializeObject<T>(_db.StringGet(redisKey));
            //this._logger.LogInformation($"执行CacheGet-Object,Key:{redisKey},Value:{result}");
            return result;
        }

        public async Task<T> ObjectGetAsync<T>(string redisKey, TimeSpan? expiry = null)
        {
            if (_bloomFilter != null)
                if (!await _bloomFilter.ContainsAsync(redisKey))
                {
                    //this._logger.LogWarning($"执行CacheGet-Object时,当前Key:{redisKey}不存在!");
                    return default;
                }
            var value = await _db.StringGetAsync(redisKey);
            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);

            return default;
        }

        public bool ObjectSet<T>(string redisKey, T redisValue, TimeSpan? expiry = null)
        {
            if (_bloomFilter != null)
                _bloomFilter.Add(redisKey);
            var json = JsonConvert.SerializeObject(redisValue);
            //this._logger.LogInformation($"执行CacheSet-Object,Key:{redisKey},Value:{json}");
            return _db.StringSet(redisKey, json, expiry);
        }

        public async Task<bool> ObjectSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null)
        {
            if (_bloomFilter != null)
                _bloomFilter.Add(redisKey);
            var json = JsonConvert.SerializeObject(redisValue);
            // this._logger.LogInformation($"执行CacheSet-Object,Key:{redisKey},Value:{json}");
            return await _db.StringSetAsync(redisKey, json, expiry);
        }

        public async Task<bool> OverlappingWork([NotNull] string resource, TimeSpan expiryTime, Func<Task> work)
        {
            resource = CreateKey(resource);
            using (RedLockFactory redisLockFactory = RedLockFactory.Create(redLockMultiplexers))
            {
                using (IRedLock redisLock = await redisLockFactory.CreateLockAsync(resource, expiryTime))
                {
                    if (redisLock.IsAcquired)
                    {
                        await work.Invoke();
                        //factory.Start();
                        // _queue.Enqueue(new RedisWatchDogDto() { ThreadId = factory.Id, Key = resource, Resource = factory, Time = expiryTime });
                        return true;
                    }
                };
                return false;
            }
        }

        public string StringGet(string redisKey, TimeSpan? expiry = null)
        {
            if (_bloomFilter != null)
                if (!_bloomFilter.Contains(redisKey))
                {
                    //this._logger.LogWarning($"执行CacheGet-String时,当前Key:{redisKey}不存在!");
                    return null;
                }
            var result = _db.StringGet(redisKey);
            //this._logger.LogInformation($"执行CacheGet-String,Key:{redisKey},Value:{result}");
            return result;
        }

        public async Task<string> StringGetAsync(string redisKey, TimeSpan? expiry = null)
        {
            //var name = _bloomFilter.Name;
            //if (!await _bloomFilter.ContainsAsync(redisKey))
            //{
            //    //this._logger.LogWarning($"执行CacheGet-String时,当前Key:{redisKey}不存在!");
            //    return null;
            //}
            var result = await _db.StringGetAsync(redisKey);
            //this._logger.LogInformation($"执行CacheGet-String,Key:{redisKey},Value:{result}");
            return result;
        }

        public async Task<long> StringIncrementAsync(string redisKey) => await _db.StringIncrementAsync(redisKey);

        public bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null)
        {
            if (_bloomFilter != null) _bloomFilter.Add(redisKey);
            //this._logger.LogInformation($"执行CacheSet-String,Key:{redisKey},Value:{redisValue}");
            return _db.StringSet(redisKey, redisValue, expiry);
        }

        public async Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null)
        {
            if (_bloomFilter != null)
                _bloomFilter.Add(redisKey);
            //this._logger.LogInformation($"执行CacheSet-String,Key:{redisKey},Value:{redisValue}");
            return await _db.StringSetAsync(redisKey, redisValue, expiry);
        }

        /// <summary>
        /// 重新设置键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string CreateKey(string key) => string.Join(":", "sempliceinstance", "RedLock", key);
    }
}