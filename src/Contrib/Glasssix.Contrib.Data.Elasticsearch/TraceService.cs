using Glasssix.Contrib.Data.Elasticsearch.Constants;
using Glasssix.Contrib.Data.Elasticsearch.Extenistions;
using Glasssix.Contrib.Data.Elasticsearch.Model;
using Glasssix.Contrib.Data.Elasticsearch.Model.Aggregate;
using Glasssix.Contrib.Data.Elasticsearch.Model.Scroll;
using Glasssix.Contrib.Data.Elasticsearch.Model.Trace;
using Glasssix.Contrib.Data.Elasticsearch.Service;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    internal class TraceService : ITraceService
    {
        private readonly IElasticClient _client;

        public TraceService(IElasticClientFactory elasticClientFactory)
        {
            _client = elasticClientFactory.CreateElasticClient(false);
        }

        public async Task<object> AggregateAsync(SimpleAggregateRequestDto query)
        {
            return await _client.AggregateTraceAsync(query);
        }

        public async Task<IEnumerable<TraceResponseDto>> GetAsync(string traceId)
        {
            return (await _client.SearchTraceAsync(new BaseRequestDto { TraceId = traceId, Page = 1, PageSize = ElasticConstant.MaxRecordCount - 1 })).Result;
        }

        public Task<PaginatedListBase<TraceResponseDto>> ListAsync(BaseRequestDto query)
        {
            return _client.SearchTraceAsync(query);
        }

        public Task<PaginatedListBase<TraceResponseDto>> ScrollAsync(BaseRequestDto query)
        {
            if (query is not ElasticsearchScrollRequestDto)
                throw new Exception("parameter: query must is type: ElasticsearchScrollRequestDto");
            return _client.SearchTraceAsync(query);
        }
    }
}