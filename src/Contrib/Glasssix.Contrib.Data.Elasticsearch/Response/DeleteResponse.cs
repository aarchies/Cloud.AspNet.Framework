namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class DeleteResponse : ResponseBase
    {
        public DeleteResponse(Nest.DeleteResponse deleteResponse) : base(deleteResponse)
        {
        }
    }
}