using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller.Middleware
{
    public class DefaultCallerMiddleware : ICallerMiddleware
    {
        private readonly Func<HttpRequestMessage, Task> _httpRequestMessageFunc;

        public DefaultCallerMiddleware(Func<HttpRequestMessage, Task> httpRequestMessageFunc)
        {
            _httpRequestMessageFunc = httpRequestMessageFunc;
        }

        public Task HandleAsync(GlasssixHttpContext GlasssixHttpContext, CallerHandlerDelegate next, CancellationToken cancellationToken = default)
        {
            _httpRequestMessageFunc.Invoke(GlasssixHttpContext.RequestMessage);
            return next();
        }
    }
}