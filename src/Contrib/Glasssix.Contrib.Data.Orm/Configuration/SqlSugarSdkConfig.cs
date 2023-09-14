using SqlSugar;
using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Orm.Configuration
{
    public class SqlSugarSdkConfig : ISqlSugarSdkConfig
    {
        /// <summary>
        /// 一些扩展层务的集成
        /// </summary>
        public ConfigureExternalServices? ConfigureExternalServices { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// 是否启用，默认启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// ORM读取自增列和主键的方式 ，建议从特性读取，如果从数据库读取需要SA等高级权限账号
        /// </summary>
        public InitKeyType InitKeyType { get; set; }

        /// <summary>
        /// 自动释放和关闭数据库连接，如果有事务事务结束时关闭，否则每次操作后关闭
        /// </summary>
        public bool IsAutoCloseConnection { get; set; }

        public Action<DiffLogModel>? OnDiffLogEvent { get; set; }
        public Action<SqlSugarException>? OnError { get; set; }
        public Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>? OnExecutingChangeSql { get; set; }
        public Action<string, SugarParameter[]>? OnLogExecuted { get; set; }
        public Action<string, SugarParameter[]>? OnLogExecuting { get; set; }
    }
}