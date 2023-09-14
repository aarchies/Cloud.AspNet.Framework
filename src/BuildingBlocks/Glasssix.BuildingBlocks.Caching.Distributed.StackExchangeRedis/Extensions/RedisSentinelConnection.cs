using StackExchange.Redis;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Extensions
{
    public class RedisSentinelConnection
    {
        /// <summary>
        /// 哨兵模式
        /// </summary>
        /// <param name="_configuration"></param>
        /// <returns></returns>

        public static ConnectionMultiplexer StackExchangeConnectRedis(string redisContion)
        {
            var configurations = ConfigurationOptions.Parse(redisContion, true);
            configurations.ResolveDns = true;
            return ConnectionMultiplexer.Connect(configurations);
        }

        public static ConnectionMultiplexer StackExchangeSentinelRedis(string redisContion)
        {
            var configurations = ConfigurationOptions.Parse(redisContion, true);
            configurations.ResolveDns = true;
            var sentinelConnection = ConnectionMultiplexer.SentinelConnectAsync(configurations).GetAwaiter().GetResult();

            ConfigurationOptions redisServiceOptions = new ConfigurationOptions();
            redisServiceOptions.ServiceName = configurations.ServiceName;   //master名称
            redisServiceOptions.Password = configurations.Password;     //master访问密码
            redisServiceOptions.AbortOnConnectFail = true;
            redisServiceOptions.DefaultDatabase = configurations.DefaultDatabase;
            ConnectionMultiplexer masterConnection = sentinelConnection.GetSentinelMasterConnection(redisServiceOptions);
            return masterConnection;
        }

        /// <summary>
        /// 单节点模式
        /// </summary>
        /// <param name="_configuration"></param>
        /// <returns></returns>
    }
}