namespace Glasssix.Contrib.Caching.Enumerations
{
    /// <summary>
    /// 缓存Key类型规则
    /// </summary>
    public enum CacheKeyType
    {
        /// <summary>
        /// 默认 全量
        /// </summary>
        None = 1,

        /// <summary>
        /// 类型的名称和键组合
        /// </summary>
        TypeName,

        /// <summary>
        /// 键入别名和键组合，格式：$｛TypeAliasName｝ ： {key｝
        /// </summary>
        TypeAlias
    }
}