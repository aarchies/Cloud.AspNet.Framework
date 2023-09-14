using Glasssix.Contrib.Caller.Extensions;
using Glasssix.Contrib.Caller.Middleware;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller.DaprClient.Internal
{
    internal static class GlasssixCallerClientBuilderExtensions
    {
        public static IGlasssixCallerClientBuilder AddConfigHttpRequestMessage(
            this IGlasssixCallerClientBuilder GlasssixCallerClientBuilder,
            Func<HttpRequestMessage, Task> httpRequestMessageFunc)
            => GlasssixCallerClientBuilder.AddMiddleware(_ => new DefaultCallerMiddleware(httpRequestMessageFunc));
    }
}