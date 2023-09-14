using DapperExtensions.Mapper;
using System;

namespace Glasssix.Contrib.Data.Orm.Dapper.Extensions
{
    /// <summary>
    /// 自定义类映射器
    /// </summary>
    public class CustomPluralizedMapper<T> : PluralizedAutoClassMapper<T> where T : class
    {
        public CustomPluralizedMapper()
        {
            //Table(tableName);
        }

        public override void Table(string tableName)
        {
            //if (!string.IsNullOrEmpty(tableName))
            //{
            TableName = "devicepersonitem_112233";
            Console.WriteLine(TableName);
            base.Table(TableName);
            // }
        }
    }
}