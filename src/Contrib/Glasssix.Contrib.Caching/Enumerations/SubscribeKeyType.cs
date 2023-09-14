namespace Glasssix.Contrib.Caching.Enumerations
{
    /// <summary>
    /// 订阅Key类型
    /// </summary>
    public enum SubscribeKeyType
    {
        /// <summary>
        /// 值类型名称
        /// </summary>
        ValueTypeFullName = 1,

        /// <summary>
        /// 值类型名称加Key
        /// </summary>
        ValueTypeFullNameAndKey = 2,

        /// <summary>
        /// 类型前缀
        /// </summary>
        SpecificPrefix = 3
    }
}