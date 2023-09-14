namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class CreateResponse : ResponseBase
    {
        public CreateResponse(Nest.CreateResponse createResponse) : base(createResponse)
        {
        }
    }
}