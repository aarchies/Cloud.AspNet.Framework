using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Glasssix.BuildingBlocks.Data.IdGenerator.NormalId
{
    internal class Snowflake
    {
        internal const long MaxWorkerId = (0b0000001 << WorkerIdBits) - 1;

        internal static readonly Snowflake Instance = new();

        internal static long SequenceMask = (0b000000000000000000000000000000000001 << SequenceBits) - 0b0000001;

        //最大机器ID
        private const int SequenceBits = 10;

        private const int TimestampLeftShift = SequenceBits + WorkerIdBits;

        //机器ID 不同的机器存放不同的id  最大现在就是 -1L ^ -1L << workerIdBits;
        private const long Twepoch = 687888001020L; //唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳

        private const int WorkerIdBits = 0b0000100; //机器码字节数。4个字节用来保存机器码(定义为Long类型会出现，最大偏移64位，所以左移64位没有意义)

        //计数器字节数，10个字节用来保存计数码
        private const int WorkerIdShift = SequenceBits; //机器码数据左移位数，就是后面计数器占用的位数

        //时间戳左移动位数就是机器码和计数器总字节数
        //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成

        private static long _lastTimestamp = -1L;
        private static long _nodeId;
        private static long _sequence = 0L;

        /// <summary>
        /// 机器码
        /// </summary>
        internal Snowflake()
        {
            var ip = Dns
            .GetHostAddresses(Dns.GetHostName())
            .Where(x => Regex.IsMatch(x.ToString(), @"^[\d]+\.[\d]+\.[\d]+\.[\d]+$"))
            .FirstOrDefault()?
            .ToString() ?? new Random().Next(0, 16).ToString();

            //Console.WriteLine("雪花节点IP:" + ip);

            ip = ip.Split('.').Last();

            long workerId = long.Parse(ip) % MaxWorkerId;

            //Console.WriteLine("雪花节点ID:" + workerId);

            if (workerId > MaxWorkerId || workerId < 0)
                throw new Exception($"节点id 不能大于 {workerId} 或者 小于 0 ");

            _nodeId = workerId;
        }

        internal long NextId()
        {
            lock (this)
            {
                long timestamp = TimeGen();
                if (_lastTimestamp == timestamp)
                { //同一微妙中生成ID
                    _sequence = _sequence + 1 & SequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (_sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = TillNextMillis(_lastTimestamp);
                    }
                }
                else
                { //不同微秒生成ID
                    _sequence = 0; //计数清0
                }
                if (timestamp < _lastTimestamp)
                { //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                    throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds",
                        _lastTimestamp - timestamp));
                }
                _lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long nextId = timestamp - Twepoch << TimestampLeftShift | _nodeId << WorkerIdShift | _sequence;
                return nextId;
            }
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private long TillNextMillis(long lastTimestamp)
        {
            long timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns></returns>
        private long TimeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}