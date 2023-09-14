using System;
using System.Threading.Tasks;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Helper
{
    public class RedisWatchDogDto
    {
        /// <summary>
        /// 锁主键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 锁资源
        /// </summary>
        public Task Resource { get; set; }

        /// <summary>
        /// 线程Id
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// 锁时间
        /// </summary>
        public TimeSpan Time { get; set; }
    }
}