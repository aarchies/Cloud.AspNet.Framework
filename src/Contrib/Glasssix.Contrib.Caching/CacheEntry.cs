using Glasssix.Contrib.Caching.Options;
using System;

namespace Glasssix.Contrib.Caching
{
    public class CacheEntry<T> : CacheEntryOptions<T>
    {
        public CacheEntry(T? value)
        {
            Value = value;
        }

        public CacheEntry(T value, DateTimeOffset absoluteExpiration) : this(value)
            => AbsoluteExpiration = absoluteExpiration;

        public CacheEntry(T value, TimeSpan absoluteExpirationRelativeToNow) : this(value)
            => AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;

        public T? Value { get; }
    }
}