using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Glasssix.Contrib.Caller.HttpClient
{
    [ExcludeFromCodeCoverage]
    public class GlasssixHttpClientBuilder : IGlasssixCallerClientBuilder, IHttpClientBuilder
    {
        public GlasssixHttpClientBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }

        public IServiceCollection Services { get; }
    }
}