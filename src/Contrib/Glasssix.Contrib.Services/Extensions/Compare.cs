using System.ComponentModel;

namespace Glasssix.Contrib.Services.Extensions
{
    public enum Compare
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equal = 1,

        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        GreaterThan = 2,

        /// <summary>
        /// 大于或等于
        /// </summary>
        [Description("大于或等于")]
        GreaterThanOrEqual = 3,

        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        LessThan = 4,

        /// <summary>
        /// 小于或等于
        /// </summary>
        [Description("小于或等于")]
        LessThanOrEqual = 5,

        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        NotEqual = 6,

        /// <summary>
        /// 忽略
        /// </summary>
        [Description("忽略")]
        Ignore = 0,
    }
}