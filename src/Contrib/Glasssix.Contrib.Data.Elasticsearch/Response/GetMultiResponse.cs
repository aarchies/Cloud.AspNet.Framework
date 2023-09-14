using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class GetMultiResponse<TDocument> : ResponseBase
        where TDocument : class
    {
        public GetMultiResponse(bool isValid, string message, List<IMultiGetHit<TDocument>>? multiGetHits = null) : base(isValid, message)
        {
            Data = multiGetHits?.Select(res => new GetMultiResponseItems<TDocument>(res.Id, res.Source)).ToList() ?? new();
        }

        public List<GetMultiResponseItems<TDocument>> Data { get; set; }
    }
}