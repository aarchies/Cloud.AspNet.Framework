using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Orm.Dapper.Runtime
{
    public interface IAutoRuntimeServices
    {
        void AddDataClounmConcurrent(string name, Dictionary<string, string> dis);

        void AddDataSourcesConcurrent(string name, string tableIndex);

        Task AutoCreate(bool OverdueDelete);

        Task<List<string>> GetRelevantTableNameAsync(string name);

        string? GetTableNameAsync(string name);

        string ToSqlClounm(string name);

        string ToUpdateModelSql(string name);
    }
}