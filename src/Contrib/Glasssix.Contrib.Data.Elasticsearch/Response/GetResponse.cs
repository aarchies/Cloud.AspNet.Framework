using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class GetResponse<TDocument> : ResponseBase
        where TDocument : class
    {
        public GetResponse(IGetResponse<TDocument> getResponse) : base(getResponse)
        {
            Document = getResponse.Source;
        }

        public TDocument Document { get; set; }
    }
}