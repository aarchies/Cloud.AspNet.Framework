namespace Glasssix.BuildingBlocks.Caching.MultilevelCache.Internal.Model
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class CacheItemModel<T>
    {
        public CacheItemModel(string key, string memoryCacheKey, bool isExist, T? value)
        {
            Key = key;
            MemoryCacheKey = memoryCacheKey;
            IsExist = isExist;
            Value = value;
        }

        public bool IsExist { get; set; }
        public string Key { get; set; }

        public string MemoryCacheKey { get; set; }
        public T? Value { get; set; }
    }
}