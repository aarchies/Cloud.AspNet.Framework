using Glasssix.Contrib.Caller.HttpClient.Extensions;
using Glasssix.Contrib.Caller.HttpClient.Internal;
using System;

namespace Glasssix.Contrib.Caller.HttpClient
{
    public abstract class HttpClientCallerBase : CallerBase
    {
        protected HttpClientCallerBase()
        {
        }

        protected HttpClientCallerBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected abstract string BaseAddress { get; set; }

        protected virtual string Prefix { get; set; } = string.Empty;

        public override void UseCallerExtension() => UseHttpClient();

        protected virtual void ConfigGlasssixCallerClient(GlasssixCallerClient callerClient)
        {
        }

        protected virtual void ConfigureHttpClient(System.Net.Http.HttpClient httpClient)
        {
        }

        protected virtual GlasssixHttpClientBuilder UseHttpClient()
        {
            var GlasssixHttpClientBuilder = CallerOptions.UseHttpClient(callerClient =>
            {
                callerClient.Prefix = Prefix;
                callerClient.BaseAddress = BaseAddress;
                callerClient.Configure = ConfigureHttpClient;
                ConfigGlasssixCallerClient(callerClient);
            });
            GlasssixHttpClientBuilder.AddConfigHttpRequestMessage(ConfigHttpRequestMessageAsync);
            return GlasssixHttpClientBuilder;
        }
    }
}