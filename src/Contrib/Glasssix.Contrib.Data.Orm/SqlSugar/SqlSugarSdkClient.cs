using Glasssix.Contrib.Data.Orm.Configuration;
using SqlSugar;
using System.Data;

namespace Glasssix.Contrib.Data.Orm.SqlSugar
{
    public class SqlSugarSdkClient : ClientBase, ISqlSugarSdkClient
    {
        private readonly ISqlSugarSdkConfig _sqlSugarSdkConfig;

        public SqlSugarSdkClient(ISqlSugarSdkConfig config) : base(config)
        {
            _sqlSugarSdkConfig = config;
        }

        public SqlSugarClient InitSqlSugarClient()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _sqlSugarSdkConfig.ConnectionString,
                DbType = _sqlSugarSdkConfig.DbType,
                IsAutoCloseConnection = _sqlSugarSdkConfig.IsAutoCloseConnection,
                InitKeyType = _sqlSugarSdkConfig.InitKeyType,
                ConfigureExternalServices = _sqlSugarSdkConfig.ConfigureExternalServices
            });

            if (_sqlSugarSdkConfig.OnDiffLogEvent != null) db.Aop.OnDiffLogEvent = _sqlSugarSdkConfig.OnDiffLogEvent;
            if (_sqlSugarSdkConfig.OnError != null) db.Aop.OnError = _sqlSugarSdkConfig.OnError;
            if (_sqlSugarSdkConfig.OnLogExecuting != null) db.Aop.OnLogExecuting = _sqlSugarSdkConfig.OnLogExecuting;
            if (_sqlSugarSdkConfig.OnLogExecuted != null) db.Aop.OnLogExecuted = _sqlSugarSdkConfig.OnLogExecuted;
            if (_sqlSugarSdkConfig.OnExecutingChangeSql != null) db.Aop.OnExecutingChangeSql = _sqlSugarSdkConfig.OnExecutingChangeSql;

            if (db.Ado.Connection.State == ConnectionState.Closed)
            {
                db.Ado.Connection.Open();
            }

            return db;
        }
    }
}