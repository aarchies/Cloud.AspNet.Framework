using Nest;
using System;

namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class SearchPaginatedResponse<TDocument> : SearchResponse<TDocument>
        where TDocument : class
    {
        public SearchPaginatedResponse(ISearchResponse<TDocument> searchResponse) : base(searchResponse)
        {
            Total = searchResponse.Hits.Count;
        }

        public SearchPaginatedResponse(int pageSize, ISearchResponse<TDocument> searchResponse) : this(searchResponse)
        {
            TotalPages = (int)Math.Ceiling(Total / (decimal)pageSize);
        }

        public long Total { get; set; }

        public int TotalPages { get; set; }
    }
}