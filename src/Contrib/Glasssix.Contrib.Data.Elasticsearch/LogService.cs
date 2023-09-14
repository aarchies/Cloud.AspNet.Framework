using Glasssix.Contrib.Caller;
using Glasssix.Contrib.Data.Elasticsearch.Constants;
using Glasssix.Contrib.Data.Elasticsearch.Extenistions;
using Glasssix.Contrib.Data.Elasticsearch.Model;
using Glasssix.Contrib.Data.Elasticsearch.Model.Aggregate;
using Glasssix.Contrib.Data.Elasticsearch.Model.LogModel;
using Glasssix.Contrib.Data.Elasticsearch.Service;
using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    internal class LogService : ILogService
    {
        private readonly ICallerFactory _callerFactory;
        private readonly IElasticClient _client;

        public LogService(IElasticClientFactory elasticClientFactory, ICallerFactory callerFactory)
        {
            _client = elasticClientFactory.CreateElasticClient(true);
            _callerFactory = callerFactory;
        }

        public async Task<object> AggregateAsync(SimpleAggregateRequestDto query)
        {
            return await _client.AggregateLogAsync(query);
        }

        public async Task<IEnumerable<MappingResponseDto>> GetMappingAsync()
        {
            return await _callerFactory.Create().GetMappingAsync(ElasticConstant.Log.IndexName);
        }

        public async Task<PaginatedListBase<LogResponseDto>> ListAsync(BaseRequestDto query)
        {
            return await _client.SearchLogAsync(query);
        }
    }
}