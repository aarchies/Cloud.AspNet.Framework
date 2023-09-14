using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal.Extensions;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal.Helpers;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Internal.Model;
using Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Options;
using Glasssix.Contrib.Caching;
using Glasssix.Contrib.Caching.Options;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control
{
    public abstract class RedisCacheClientBase : DistributedCacheClientBase
    {
        protected static readonly Guid UniquelyIdentifies = Guid.NewGuid();
        protected readonly JsonSerializerOptions GlobalJsonSerializerOptions;
        protected IDatabase Db;
        protected CacheEntryOptions GlobalCacheEntryOptions;
        protected CacheOptions GlobalCacheOptions;
        protected List<RedLockMultiplexer> redLockMultiplexers;
        protected ISubscriber Subscriber;

        protected RedisCacheClientBase(RedisConfigurationOptions redisConfigurationOptions,
            JsonSerializerOptions? jsonSerializerOptions)
        {
            GlobalCacheOptions = redisConfigurationOptions.GlobalCacheOptions;
            var redisConfiguration = GetRedisConfigurationOptions(redisConfigurationOptions);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(redisConfiguration);
            Db = connection.GetDatabase();
            Subscriber = connection.GetSubscriber();
            redLockMultiplexers = new List<RedLockMultiplexer> { connection };
            GlobalJsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions();
            //GlobalJsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions().EnableDynamicTypes();

            GlobalCacheEntryOptions = new CacheEntryOptions
            {
                AbsoluteExpiration = redisConfiguration.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = redisConfiguration.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = redisConfiguration.SlidingExpiration
            };
        }

        internal static DataCacheModel MapMetadata(
            string key,
            RedisValue[] results)
        {
            RedisValue data = results.Length > 2 ? results[2] : RedisValue.Null;
            var absoluteExpirationTicks = (long?)results[0];
            if (absoluteExpirationTicks is null or Const.DEADLINE_LASTING)
                absoluteExpirationTicks = null;

            var slidingExpirationTicks = (long?)results[1];
            if (slidingExpirationTicks is null or Const.DEADLINE_LASTING)
                slidingExpirationTicks = null;
            return new DataCacheModel(key, absoluteExpirationTicks, slidingExpirationTicks, data);
        }

        internal List<DataCacheModel> GetList(IEnumerable<string> keys, bool getData)
        {
            string script = getData ? Const.GET_LIST_SCRIPT : Const.GET_EXPIRATION_VALUE_SCRIPT;
            var arrayRedisResult = Db
                .ScriptEvaluate(script, keys.Select(key => (RedisKey)key).ToArray())
                .ToDictionary();
            return GetListByArrayRedisResult(arrayRedisResult, getData);
        }

        internal async Task<List<DataCacheModel>> GetListAsync(IEnumerable<string> keys, bool getData)
        {
            string script = getData ? Const.GET_LIST_SCRIPT : Const.GET_EXPIRATION_VALUE_SCRIPT;
            var arrayRedisResult = (await Db
                    .ScriptEvaluateAsync(script, keys.Select(key => (RedisKey)key).ToArray()).ConfigureAwait(false))
                .ToDictionary();
            return GetListByArrayRedisResult(arrayRedisResult, getData);
        }

        internal List<DataCacheModel> GetListByKeyPattern(string keyPattern)
            => GetListCoreByKeyPattern(keyPattern, (script, parameters) => Db.ScriptEvaluate(LuaScript.Prepare(script), parameters)
                .ToDictionary());

        internal Task<List<DataCacheModel>> GetListByKeyPatternAsync(string keyPattern)
            => Task.FromResult(GetListCoreByKeyPattern(keyPattern, (script, parameters) => Db
                .ScriptEvaluateAsync(LuaScript.Prepare(script), parameters)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
                .ToDictionary()));

        internal void RefreshCore(List<DataCacheModel> models, CancellationToken token = default)
        {
            var awaitRefreshOptions = GetKeyAndExpireList(models, token);
            if (awaitRefreshOptions.Count > 0)
            {
                Db.ScriptEvaluate(Const.SET_EXPIRE_SCRIPT,
                    awaitRefreshOptions.Select(item => item.Key).GetRedisKeys(),
                    awaitRefreshOptions.Select(item => (RedisValue)(item.Value?.TotalSeconds ?? -1)).ToArray()
                );
            }
        }

        internal async Task RefreshCoreAsync(
            List<DataCacheModel> models,
            CancellationToken token = default)
        {
            var awaitRefreshOptions = GetKeyAndExpireList(models, token);
            if (awaitRefreshOptions.Count > 0)
            {
                await Db.ScriptEvaluateAsync(Const.SET_EXPIRE_SCRIPT,
                    awaitRefreshOptions.Select(item => item.Key).GetRedisKeys(),
                    awaitRefreshOptions.Select(item => (RedisValue)(item.Value?.TotalSeconds ?? -1)).ToArray()
                ).ConfigureAwait(false);
            }
        }

        protected static PublishOptions GetAndCheckPublishOptions(string channel, Action<PublishOptions> setup)
        {
            //ArgumentNullException.ThrowIfNull(channel);

            //ArgumentNullException.ThrowIfNull(setup);

            var options = new PublishOptions(UniquelyIdentifies);
            setup.Invoke(options);

            //GlasssixArgumentException.ThrowIfNullOrWhiteSpace(options.Key);

            return options;
        }

        protected T? ConvertToValue<T>(RedisValue value, out bool isExist)
        {
            if (value.HasValue && !value.IsNullOrEmpty)
            {
                isExist = true;
                return value.ConvertToValue<T>(GlobalJsonSerializerOptions);
            }

            isExist = false;
            return default;
        }

        protected CacheEntryOptions GetCacheEntryOptions(CacheEntryOptions? options = null)
            => options ?? GlobalCacheEntryOptions;

        protected CacheOptions GetCacheOptions(Action<CacheOptions>? action)
        {
            if (action != null)
            {
                CacheOptions cacheOptions = new CacheOptions();
                action.Invoke(cacheOptions);
                return cacheOptions;
            }
            return GlobalCacheOptions;
        }

        private static List<KeyValuePair<string, TimeSpan?>> GetKeyAndExpireList(
            List<DataCacheModel> models,
            CancellationToken token)
        {
            List<KeyValuePair<string, TimeSpan?>> list = new();

            DateTimeOffset? creationTime = DateTimeOffset.UtcNow;
            foreach (var model in models)
            {
                var res = model.GetExpiration(creationTime, token);
                if (res.State)
                {
                    list.Add(new KeyValuePair<string, TimeSpan?>(model.Key, res.Expire));
                }
            }
            return list;
        }

        private static List<DataCacheModel> GetListByArrayRedisResult(Dictionary<string, RedisResult> arrayRedisResult, bool getData)
        {
            List<DataCacheModel> list = new List<DataCacheModel>();
            foreach (var redisResult in arrayRedisResult)
            {
                var byteArray = (RedisValue[])redisResult.Value;
                list.Add(getData ? MapMetadataByAutomatic(redisResult.Key, byteArray) : MapMetadata(redisResult.Key, byteArray));
            }
            return list;
        }

        private static List<DataCacheModel> GetListCoreByKeyPattern(
            string keyPattern,
            Func<string, object, Dictionary<string, RedisResult>> func)
        {
            var arrayRedisResult = func.Invoke(
                Const.GET_KEY_AND_VALUE_SCRIPT,
                new
                {
                    keypattern = keyPattern
                });

            List<DataCacheModel> list = new List<DataCacheModel>();
            foreach (var redisResult in arrayRedisResult)
            {
                var byteArray = (RedisValue[])redisResult.Value;
                list.Add(MapMetadataByAutomatic(redisResult.Key, byteArray));
            }
            return list;
        }

        private static RedisConfigurationOptions GetRedisConfigurationOptions(RedisConfigurationOptions redisConfigurationOptions)
        {
            if (redisConfigurationOptions.Servers.Any())
                return redisConfigurationOptions;

            return new RedisConfigurationOptions()
            {
                Servers = new List<RedisServerOptions>()
            {
                new()
            }
            };
        }

        private static DataCacheModel MapMetadataByAutomatic(string key, RedisValue[] results)
        {
            long? absoluteExpiration = null;
            long? slidingExpiration = null;
            RedisValue data = RedisValue.Null;

            for (int index = 0; index < results.Length; index += 2)
            {
                if (results[index] == Const.ABSOLUTE_EXPIRATION_KEY)
                {
                    var absoluteExpirationTicks = (long?)results[index + 1];
                    if (absoluteExpirationTicks.HasValue && absoluteExpirationTicks.Value != Const.DEADLINE_LASTING)
                    {
                        absoluteExpiration = absoluteExpirationTicks.Value;
                    }
                }
                else if (results[index] == Const.SLIDING_EXPIRATION_KEY)
                {
                    var slidingExpirationTicks = (long?)results[index + 1];
                    if (slidingExpirationTicks.HasValue && slidingExpirationTicks.Value != Const.DEADLINE_LASTING)
                    {
                        slidingExpiration = slidingExpirationTicks;
                    }
                }
                else if (results[index] == Const.DATA_KEY)
                {
                    data = results[index + 1];
                }
            }
            return new DataCacheModel(key, absoluteExpiration, slidingExpiration, data);
        }
    }
}