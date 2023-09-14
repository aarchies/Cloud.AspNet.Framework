using Microsoft.Extensions.Logging;
using System;
using System.Collections;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Extensions.Filter
{
    /// <summary>
    /// 布隆查询器-解决穿透问题 集合中有的缓存可能没有 集合中没有的缓存肯定没有 InMemory
    /// </summary>
    public class BloomFilter
    {
        public readonly ILogger<BloomFilter> _logger;

        //public readonly
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_BitIndexCount">Hash次数</param>
        /// <param name="_BloomArryLength">过滤器长度</param>
        public BloomFilter(ILoggerFactory loggerFactory, int _BitIndexCount, int _BloomArryLength)
        {
            _BloomArray = new BitArray(_BloomArryLength);
            BloomArryLength = _BloomArryLength;
            BitIndexCount = _BitIndexCount;
            _logger = loggerFactory.CreateLogger<BloomFilter>();
            _logger.LogInformation("布隆拦截器初始化");
        }

        public BitArray _BloomArray { get; set; }
        public BloomFilter _bloomFilter { get; set; }
        private int BitIndexCount { get; set; }
        private int BloomArryLength { get; set; }

        /// <summary>
        /// 参与集合-同key hash值会被覆盖 省去remove操作
        /// </summary>
        /// <param name="Id">标识Id</param>
        public void Add(string Id)
        {
            var hashCode = GetHashCode(Id);
            Random random = new Random(hashCode);
            for (int i = 0; i < BitIndexCount; i++)
            {
                var num = random.Next(BloomArryLength - 1);
                _BloomArray[num] = true;
            }
            _logger.LogInformation($"Id：{Id} 加入布隆集合 value：{_BloomArray}");
        }

        public void Dispose()
        {
            _BloomArray.Not();
            BitIndexCount = 0;
            BloomArryLength = 0;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(string Id)
        {
            var hashCode = GetHashCode(Id);
            Random random = new Random(hashCode);
            for (int i = 0; i < BitIndexCount; i++)
            {
                var num = random.Next(BloomArryLength - 1);
                if (!_BloomArray[num]) return false;
            }
            return true;
        }

        private int GetHashCode(object code)
        {
            return code.GetHashCode();
        }
    }
}