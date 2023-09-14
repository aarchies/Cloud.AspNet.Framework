using Microsoft.Extensions.Configuration;

namespace Glasssix.Utils.Configuration
{
    public interface IConfigurationApi
    {
        public IConfiguration Get(string appId);
    }
}