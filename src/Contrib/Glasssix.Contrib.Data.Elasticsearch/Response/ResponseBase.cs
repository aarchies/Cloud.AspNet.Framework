using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class ResponseBase
    {
        public ResponseBase(IResponse response) : this(response.IsValid, response.IsValid ? "success" : response.ServerError?.ToString() ?? string.Empty)
        {
        }

        protected ResponseBase(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        public bool IsValid { get; }

        public string Message { get; }
    }
}