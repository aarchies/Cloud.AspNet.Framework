namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class ExistsResponse : ResponseBase
    {
        public ExistsResponse(Nest.ExistsResponse existsResponse) : base(
            existsResponse.IsValid || existsResponse.ApiCall.HttpStatusCode == 404,
            existsResponse.IsValid || existsResponse.ApiCall.HttpStatusCode == 404 ? "success" : existsResponse.ServerError?.ToString() ?? string.Empty)
        {
            Exists = existsResponse.Exists;
        }

        public bool Exists { get; }
    }
}