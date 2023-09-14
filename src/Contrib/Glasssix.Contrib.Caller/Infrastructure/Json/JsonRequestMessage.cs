using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Glasssix.Contrib.Caller.Infrastructure.Json
{
    public class JsonRequestMessage : IRequestMessage
    {
        private readonly JsonSerializerOptions? _jsonSerializerOptions;

        public JsonRequestMessage(JsonSerializerOptions? jsonSerializerOptions = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions ?? GlasssixIocApp.GetJsonSerializerOptions();
        }

        public virtual void ProcessHttpRequestMessage(HttpRequestMessage requestMessage)
        {
        }

        public virtual void ProcessHttpRequestMessage<TRequest>(HttpRequestMessage requestMessage, TRequest data)
        {
            ProcessHttpRequestMessage(requestMessage);
            requestMessage.Content = JsonContent.Create(data, options: _jsonSerializerOptions);
        }
    }
}