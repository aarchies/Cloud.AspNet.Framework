using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Caching.TypeAlias.Options
{
    /// <summary>
    /// 类型别名选项
    /// </summary>
    public class TypeAliasOptions
    {
        /// <summary>
        /// 获取所有别名
        /// </summary>
        public Func<Dictionary<string, string>>? GetAllTypeAliasFunc { get; set; }

        /// <summary>
        /// 刷新TypeAlias最小间隔时间
        /// default: 30s
        /// </summary>
        public long RefreshTypeAliasInterval { get; set; } = 30;
    }
}