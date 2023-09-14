using Glasssix.Contrib.Data.Storage.Model;
using Glasssix.Contrib.Data.Storage.Model.Aggregate;
using Glasssix.Contrib.Data.Storage.Model.Trace;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Storage.Service
{
    public interface ITraceService
    {
        /// <summary>
        /// ����ѯ����:Count,Sum,Avg��DistinctCount��������Ϊdouble, DateHistogram����IEnumerable<KeyValuePair<double, long>>��GroupBy����IEnumerable<string>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<object> AggregateAsync(SimpleAggregateRequestDto query);

        Task<IEnumerable<TraceResponseDto>> GetAsync(string traceId);

        Task<PaginatedListBase<TraceResponseDto>> ListAsync(BaseRequestDto query);

        Task<PaginatedListBase<TraceResponseDto>> ScrollAsync(BaseRequestDto query);
    }
}