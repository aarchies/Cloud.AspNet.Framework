using System;

namespace Glasssix.Contrib.Caching.Options
{
    /// <summary>
    /// 缓存周期配置选项
    /// </summary>
    public class CacheEntryOptions
    {
        private TimeSpan? _absoluteExpirationRelativeToNow;
        private TimeSpan? _slidingExpiration;

        public CacheEntryOptions()
        { }

        public CacheEntryOptions(DateTimeOffset? absoluteExpiration)
                    => AbsoluteExpiration = absoluteExpiration;

        public CacheEntryOptions(TimeSpan? absoluteExpirationRelativeToNow)
                    => AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;

        /// <summary>
        /// 绝对过期时间
        /// 与AbsoluteExpirationRelativeToNow共存时，请首先使用AbsoluteexpirationRelativeToNow
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; set; }

        /// <summary>
        /// 相对过期时间.
        /// 与AbsoluteExpiration共存时，请先使用AbsoluteExpirationRelativeToNow
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow
        {
            get => _absoluteExpirationRelativeToNow;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(AbsoluteExpirationRelativeToNow),
                        value, "相对值需为正.");
                }

                _absoluteExpirationRelativeToNow = value;
            }
        }

        /// <summary>
        /// 滑动过期时长
        /// 将条目生存期延长到绝对过期（如果设置）之后
        /// </summary>
        public TimeSpan? SlidingExpiration
        {
            get => _slidingExpiration;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(SlidingExpiration),
                        value,
                        "相对值需为正.");
                }
                _slidingExpiration = value;
            }
        }
    }

    public class CacheEntryOptions<T> : CacheEntryOptions
    {
        public CacheEntryOptions()
        {
        }

        public CacheEntryOptions(DateTimeOffset? absoluteExpiration) : base(absoluteExpiration)
        {
        }

        public CacheEntryOptions(TimeSpan? absoluteExpirationRelativeToNow) : base(absoluteExpirationRelativeToNow)
        {
        }

        public Action<T?>? ValueChanged { get; set; }
    }
}