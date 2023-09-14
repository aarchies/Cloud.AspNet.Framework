using Glasssix.Contrib.Data.Storage.Model;
using Glasssix.Contrib.Data.Storage.Model.Aggregate;
using Glasssix.Contrib.Data.Storage.Model.LogModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Storage.Service
{
    public interface ILogService
    {
        /// <summary>
        /// when query type: Count,Sum,Avg and DistinctCount return type is double, DateHistogram return IEnumerable<KeyValuePair<double, long>> ,GroupBy return IEnumerable<string>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<object> AggregateAsync(SimpleAggregateRequestDto query);

        Task<IEnumerable<MappingResponseDto>> GetMappingAsync();

        Task<PaginatedListBase<LogResponseDto>> ListAsync(BaseRequestDto query);
    }
}