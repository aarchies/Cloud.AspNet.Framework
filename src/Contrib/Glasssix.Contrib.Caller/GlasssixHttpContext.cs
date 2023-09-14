using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller
{
    public class GlasssixHttpContext
    {
        private readonly Func<HttpRequestMessage>? _httpRequestMessageFunc;

        private readonly IResponseMessage _responseMessage;
        private HttpRequestMessage? _requestMessage;

        public GlasssixHttpContext(IResponseMessage responseMessage, HttpRequestMessage requestMessage)
            : this(responseMessage)
        {
            _requestMessage = requestMessage;
        }

        public GlasssixHttpContext(IResponseMessage responseMessage, Func<HttpRequestMessage>? httpRequestMessageFunc)
            : this(responseMessage)
        {
            if (httpRequestMessageFunc != null) _httpRequestMessageFunc = httpRequestMessageFunc;
            else _requestMessage = new HttpRequestMessage();
        }

        private GlasssixHttpContext(IResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public HttpRequestMessage RequestMessage => _requestMessage ??= _httpRequestMessageFunc!.Invoke();

        public HttpResponseMessage? ResponseMessage { get; internal set; }

        internal Task ProcessResponseAsync(CancellationToken cancellationToken = default)
            => _responseMessage.ProcessResponseAsync(ResponseMessage!, cancellationToken);

        internal Task<TResponse?> ProcessResponseAsync<TResponse>(CancellationToken cancellationToken = default)
            => _responseMessage.ProcessResponseAsync<TResponse>(ResponseMessage!, cancellationToken);
    }
}