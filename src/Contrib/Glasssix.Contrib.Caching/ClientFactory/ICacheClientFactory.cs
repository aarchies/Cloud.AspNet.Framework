using Glasssix.Contrib.Data;

namespace Glasssix.Contrib.Caching.ClientFactory
{
    public interface ICacheClientFactory<out TService> : IGlasssixFactory<TService> where TService : class
    {
    }
}