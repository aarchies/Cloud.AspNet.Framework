using SqlSugar;

namespace Glasssix.Contrib.Data.Orm.SqlSugar
{
    public interface ISqlSugarSdkClient
    {
        SqlSugarClient InitSqlSugarClient();
    }
}