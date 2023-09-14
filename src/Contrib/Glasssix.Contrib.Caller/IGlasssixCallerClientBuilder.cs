using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.Contrib.Caller
{
    public interface IGlasssixCallerClientBuilder
    {
        string Name { get; }
        IServiceCollection Services { get; }
    }
}