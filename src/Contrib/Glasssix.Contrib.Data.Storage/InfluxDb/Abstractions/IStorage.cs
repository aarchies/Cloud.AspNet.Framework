using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Storage.InfluxDb.Abstractions
{
    public interface IStorage
    {
        Task Apply<T>(T model) where T : class;

        Task<List<T>> List<T>();
    }
}