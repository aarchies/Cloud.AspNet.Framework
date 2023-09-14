using Glasssix.Contrib.Caller.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Glasssix.Contrib.Caller.HttpClient.Extensions
{
    public static class CallerOptionsExtensions
    {
        public static GlasssixHttpClientBuilder UseHttpClient(this CallerOptionsBuilder callerOptionsBuilder)
            => callerOptionsBuilder.UseHttpClientCore(null);

        public static GlasssixHttpClientBuilder UseHttpClient(this CallerOptionsBuilder callerOptionsBuilder,
            Action<GlasssixHttpClient> clientConfigure)
        {
            //GlasssixArgumentException.ThrowIfNull(clientConfigure);

            return callerOptionsBuilder.UseHttpClientCore(clientConfigure);
        }

        private static GlasssixHttpClientBuilder UseHttpClientCore(this CallerOptionsBuilder callerOptionsBuilder,
            Action<GlasssixHttpClient>? clientConfigure)
        {
            callerOptionsBuilder.Services.AddHttpClient(callerOptionsBuilder.Name);

            callerOptionsBuilder.Services.AddCaller(callerOptionsBuilder.Name, serviceProvider =>
            {
                var GlasssixHttpClient = new GlasssixHttpClient();
                clientConfigure?.Invoke(GlasssixHttpClient);
                var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(callerOptionsBuilder.Name);
                GlasssixHttpClient.ConfigureHttpClient(httpClient);
                return new HttpClientCaller(
                    httpClient,
                    serviceProvider,
                    callerOptionsBuilder.Name,
                    GlasssixHttpClient.Prefix!,
                    GlasssixHttpClient.RequestMessageFactory,
                    GlasssixHttpClient.ResponseMessageFactory);
            });
            return new GlasssixHttpClientBuilder(callerOptionsBuilder.Services, callerOptionsBuilder.Name);
        }
    }
}