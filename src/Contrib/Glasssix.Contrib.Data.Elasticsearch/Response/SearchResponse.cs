using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class SearchResponse<TDocument> : ResponseBase
        where TDocument : class
    {
        public SearchResponse(ISearchResponse<TDocument> searchResponse) : base(searchResponse)
        {
            Data = searchResponse.Hits.Select(hit => hit.Source).ToList();
        }

        public List<TDocument> Data { get; }
    }
}