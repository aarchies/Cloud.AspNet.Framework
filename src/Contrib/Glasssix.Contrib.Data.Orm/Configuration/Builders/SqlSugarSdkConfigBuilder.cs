using Dapper.Contrib.Extensions;
using SqlSugar;
using System;
using System.Linq;

namespace Glasssix.Contrib.Data.Orm.Configuration.Builders
{
    public class SqlSugarSdkConfigBuilder : ISqlSugarSdkConfigBuilder
    {
        private string? _connectionString;
        private DbType _dbType = DbType.MySql;
        private bool _enabled = true;
        private InitKeyType _initKeyType = InitKeyType.Attribute;
        private bool _isAutoCloseConnection = false;

        public ISqlSugarSdkConfig Build()
        {
            SqlSugarSdkConfig config = new SqlSugarSdkConfig
            {
                Enabled = _enabled,
                ConnectionString = _connectionString!,
                DbType = _dbType,
                InitKeyType = _initKeyType,
                IsAutoCloseConnection = _isAutoCloseConnection,
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (property, column) =>
                    {
                        var attributes = property.GetCustomAttributes(true);//get all attributes

                        if (attributes.Any(it => it is KeyAttribute))// by attribute set primarykey
                        {
                            column.IsPrimarykey = true;
                        }
                    },
                    EntityNameService = (type, entity) =>
                    {
                        var attributes = type.GetCustomAttributes(true);
                        if (attributes.Any(it => it is TableAttribute))
                        {
                            entity.DbTableName = (attributes!.First(it => it is TableAttribute) as TableAttribute)!.Name;
                        }
                    }
                }
            };

            return config;
        }

        public ISqlSugarSdkConfigBuilder Enable(bool enabled)
        {
            _enabled = enabled;
            return this;
        }

        public ISqlSugarSdkConfigBuilder SetConnectionString(string connentionString)
        {
            if (string.IsNullOrEmpty(connentionString)) throw new ArgumentNullException(nameof(connentionString));

            _connectionString = connentionString;
            return this;
        }

        public ISqlSugarSdkConfigBuilder SetDbType(DbType dbType)
        {
            _dbType = dbType;
            return this;
        }

        public ISqlSugarSdkConfigBuilder SetInitKeyType(InitKeyType initKeyType)
        {
            _initKeyType = initKeyType;
            return this;
        }

        public ISqlSugarSdkConfigBuilder WithAutoCloseConnection()
        {
            _isAutoCloseConnection = true;
            return this;
        }
    }
}