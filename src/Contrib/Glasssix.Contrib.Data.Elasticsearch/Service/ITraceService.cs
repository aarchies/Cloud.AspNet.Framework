using Glasssix.Contrib.Data.Elasticsearch.Model;
using Glasssix.Contrib.Data.Elasticsearch.Model.Aggregate;
using Glasssix.Contrib.Data.Elasticsearch.Model.Trace;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Elasticsearch.Service
{
    public interface ITraceService
    {
        /// <summary>
        /// 当查询类型:Count,Sum,Avg和DistinctCount返回类型为double, DateHistogram返回IEnumerable<KeyValuePair<double, long>>，GroupBy返回IEnumerable<string>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<object> AggregateAsync(SimpleAggregateRequestDto query);

        Task<IEnumerable<TraceResponseDto>> GetAsync(string traceId);

        Task<PaginatedListBase<TraceResponseDto>> ListAsync(BaseRequestDto query);

        Task<PaginatedListBase<TraceResponseDto>> ScrollAsync(BaseRequestDto query);
    }
}