using Glasssix.Contrib.Data.Elasticsearch.Response;
using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch.Response.Document
{
    public class CountDocumentResponse : ResponseBase
    {
        public CountDocumentResponse(CountResponse response) : base(response) => Count = response.Count;

        public long Count { get; }
    }
}