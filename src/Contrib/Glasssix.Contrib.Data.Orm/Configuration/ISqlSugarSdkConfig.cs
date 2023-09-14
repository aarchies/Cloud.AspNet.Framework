using Glasssix.Contrib.Data.Orm.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Orm.Configuration
{
    public interface ISqlSugarSdkConfig : ISdkConfig
    {
        ConfigureExternalServices? ConfigureExternalServices { get; set; }
        string? ConnectionString { get; set; }
        DbType DbType { get; set; }
        InitKeyType InitKeyType { get; set; }
        bool IsAutoCloseConnection { get; set; }
        Action<DiffLogModel>? OnDiffLogEvent { get; set; }
        Action<SqlSugarException>? OnError { get; set; }
        Func<string, SugarParameter[], KeyValuePair<string, SugarParameter[]>>? OnExecutingChangeSql { get; set; }
        Action<string, SugarParameter[]>? OnLogExecuted { get; set; }
        Action<string, SugarParameter[]>? OnLogExecuting { get; set; }
    }
}