using Glasssix.Contrib.Caching.Options;

namespace Glasssix.BuildingBlocks.Caching.MultilevelCache.Internal.Options
{
    internal class SetOptions<T>
    {
        public string? FormattedKey { get; set; }

        public CacheEntryOptions? MemoryCacheEntryOptions { get; set; }
        public T? Value { get; set; }
    }
}