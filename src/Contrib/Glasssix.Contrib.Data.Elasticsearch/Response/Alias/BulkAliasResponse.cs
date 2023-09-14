using Glasssix.Contrib.Data.Elasticsearch.Response;

namespace Glasssix.Contrib.Data.Elasticsearch.Response.Alias
{
    public class BulkAliasResponse : ResponseBase
    {
        public BulkAliasResponse(Nest.BulkAliasResponse bulkAliasResponse) : base(bulkAliasResponse)
        {
        }
    }
}