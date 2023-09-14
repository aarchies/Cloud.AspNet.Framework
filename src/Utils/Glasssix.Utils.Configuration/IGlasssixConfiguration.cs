using Microsoft.Extensions.Configuration;

namespace Glasssix.Utils.Configuration
{
    public interface IGlasssixConfiguration
    {
        public IConfigurationApi ConfigurationApi { get; }
        public IConfiguration Local { get; }
    }
}