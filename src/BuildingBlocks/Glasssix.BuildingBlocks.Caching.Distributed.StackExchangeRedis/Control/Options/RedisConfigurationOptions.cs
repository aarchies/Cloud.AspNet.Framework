using Glasssix.Contrib.Caching.Enumerations;
using Glasssix.Contrib.Caching.Options;
using StackExchange.Redis;
using System.Collections.Generic;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control.Options
{
    public class RedisConfigurationOptions : CacheEntryOptions
    {
        /// <summary>
        /// 获取或设置是否应通过TimeoutException显式通知连接/配置超时
        /// </summary>
        public bool AbortOnConnectFail { get; set; }

        /// <summary>
        /// 指示是否应允许管理员操作
        /// </summary>
        public bool AllowAdmin { get; set; }

        /// <summary>
        ///指定系统应允许异步操作的时间（以毫秒为单位）（默认值：5000)
        /// </summary>
        public int AsyncTimeout { get; set; } = 5000;

        /// <summary>
        /// 自动编码和解码频道.
        /// </summary>
        public string ChannelPrefix { get; set; } = string.Empty;

        /// <summary>
        /// 用于所有连接的客户端名称
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// 如果没有服务器及时响应，则重复初始连接周期的次数
        /// </summary>
        public int ConnectRetry { get; set; } = 3;

        /// <summary>
        /// 指定应允许连接的时间（以毫秒为单位）（除非SyncTimeout更高，否则默认为5秒）
        /// </summary>
        public int ConnectTimeout { get; set; } = 5000;

        /// <summary>
        ///指定在不带任何参数的情况下调用ConnectionMultiplexer.GetDatabase（）时要使用的默认数据库
        /// </summary>
        public int DefaultDatabase { get; set; }

        /// <summary>
        /// 默认Key存储规则
        /// </summary>
        public CacheOptions GlobalCacheOptions { get; set; } = new CacheOptions()
        {
            CacheKeyType = CacheKeyType.TypeName
        };

        /// <summary>
        /// 用于与服务器进行身份验证的密码
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 使用的代理类型（如有）；例如Proxy.Twemproxy
        /// </summary>
        public Proxy Proxy { get; set; } = Proxy.None;

        /// <summary>
        /// Gets the servers.
        /// </summary>
        public List<RedisServerOptions> Servers { get; set; } = new List<RedisServerOptions>();

        /// <summary>
        /// 指示是否应加密连接
        /// </summary>
        public bool Ssl { get; set; }

        /// <summary>
        /// 指定系统应允许同步操作的时间（以毫秒为单位）（默认值为5秒）
        /// </summary>
        public int SyncTimeout { get; set; } = 1000;

        public static implicit operator ConfigurationOptions(RedisConfigurationOptions options)
        {
            var configurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = options.AbortOnConnectFail,
                AllowAdmin = options.AllowAdmin,
                AsyncTimeout = options.AsyncTimeout,
                ChannelPrefix = options.ChannelPrefix,
                ClientName = options.ClientName,
                ConnectRetry = options.ConnectRetry,
                ConnectTimeout = options.ConnectTimeout,
                DefaultDatabase = options.DefaultDatabase,
                Password = options.Password,
                Proxy = options.Proxy,
                Ssl = options.Ssl,
                SyncTimeout = options.SyncTimeout
            };

            foreach (var server in options.Servers)
            {
                configurationOptions.EndPoints.Add(server.Host, server.Port);
            }
            return configurationOptions;
        }
    }
}