using Glasssix.Contrib.Data.Elasticsearch.Response;

namespace Glasssix.Contrib.Data.Elasticsearch.Response.Index
{
    public class CreateIndexResponse : ResponseBase
    {
        public CreateIndexResponse(Nest.CreateIndexResponse createIndexResponse) : base(createIndexResponse)
        {
        }
    }
}