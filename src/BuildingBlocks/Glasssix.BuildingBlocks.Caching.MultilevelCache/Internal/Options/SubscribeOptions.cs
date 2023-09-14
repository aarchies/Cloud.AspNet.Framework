using System;

namespace Glasssix.BuildingBlocks.Caching.MultilevelCache.Internal.Options
{
    internal class SubscribeOptions<T>
    {
        public Action<T?>? ValueChanged { get; set; }
    }
}