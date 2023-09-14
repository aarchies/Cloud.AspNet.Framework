namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class CreateMultiResponse : BulkResponse
    {
        public CreateMultiResponse(Nest.BulkResponse bulkResponse) : base(bulkResponse)
        {
        }
    }
}