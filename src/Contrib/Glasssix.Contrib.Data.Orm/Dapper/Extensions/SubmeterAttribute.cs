using System;

namespace Glasssix.Contrib.Data.Orm.Dapper.Extensions
{
    /// <summary>
    /// 分表特性 （按月/25日）
    /// </summary>
    public class SubmeterAttribute : Attribute
    {
        public SubmeterAttribute(string? parm = null)
        {
            if (parm == null)
                Time = new DateTime(2022, 7, 10).ToString("yyyyMMdd");
        }

        public string? Time { get; set; }
    }
}