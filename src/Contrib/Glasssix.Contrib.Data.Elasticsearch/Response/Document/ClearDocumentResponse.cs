using Glasssix.Contrib.Data.Elasticsearch.Response;
using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch.Response.Document
{
    public class ClearDocumentResponse : ResponseBase
    {
        public ClearDocumentResponse(DeleteByQueryResponse response) : base(response)
        {
        }
    }
}