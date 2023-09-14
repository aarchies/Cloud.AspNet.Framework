using Skoruba.IdentityServer4.Admin.EntityFramework.Configuration.Configuration;
using System.Reflection;
using MySqlMigrationAssembly = Glasssix.Contrib.Authentication.EntityFramework.MySql.Helpers.MigrationAssembly;

//using PostgreSQLMigrationAssembly = IdentityAuthServer.Admin.EntityFramework.PostgreSQL.Helpers.MigrationAssembly;
//using SqlMigrationAssembly = IdentityAuthServer.Admin.EntityFramework.SqlServer.Helpers.MigrationAssembly;

namespace Glasssix.BuildingBlocks.Authentication.AdminConsole.Configuration.Database
{
    public static class MigrationAssemblyConfiguration
    {
        public static string GetMigrationAssemblyByProvider(DatabaseProviderConfiguration databaseProvider)
        {
            return databaseProvider.ProviderType switch
            {
                //DatabaseProviderType.SqlServer => typeof(SqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
                //DatabaseProviderType.PostgreSQL => typeof(PostgreSQLMigrationAssembly).GetTypeInfo()
                //    .Assembly.GetName()
                //    .Name,
                DatabaseProviderType.MySql => typeof(MySqlMigrationAssembly).GetTypeInfo().Assembly.GetName().Name,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}