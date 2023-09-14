using Glasssix.Contrib.Data.Orm.Configuration;
using Glasssix.Contrib.Data.Orm.Configuration.Builders;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Diagnostics;

namespace Glasssix.Contrib.Data.Orm.SqlSugar.SqlSugar
{
    public static class SqlSugarHelper
    {
        public static string? _con;

        public static IServiceCollection AddSqlSugar(this IServiceCollection serviceCollection, string con)
        {
            _con = con;
            return serviceCollection;
        }

        public static SqlSugarClient Instance => GetClient();

        public static SqlSugarClient GetClient()
        {
            ISqlSugarSdkConfig config = new SqlSugarSdkConfigBuilder()
                .Enable(true)
                .SetDbType(DbType.MySql)
                .SetConnectionString(_con!)
                .SetInitKeyType(InitKeyType.Attribute)
                .WithAutoCloseConnection()
                .Build();

#if DEBUG || DEV
            config.OnLogExecuting = (sql, pars) =>
            {
                Trace.WriteLine(sql + "\r\n");
            };
#endif
            SqlSugarSdkClient client = new SqlSugarSdkClient(config);

            return client.InitSqlSugarClient();
        }
    }
}