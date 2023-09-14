using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.Contrib.Caller.DaprClient
{
    public class GlasssixDaprClientBuilder : IGlasssixCallerClientBuilder
    {
        public GlasssixDaprClientBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }
        public IServiceCollection Services { get; }
    }
}