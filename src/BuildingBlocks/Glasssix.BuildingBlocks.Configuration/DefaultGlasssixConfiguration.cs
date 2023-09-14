using Glasssix.Utils.Configuration;
using Microsoft.Extensions.Configuration;

namespace Glasssix.BuildingBlocks.Configuration
{
    public class DefaultGlasssixConfiguration : IGlasssixConfiguration
    {
        private readonly IConfiguration _configuration;

        public DefaultGlasssixConfiguration(IConfiguration configuration, IConfigurationApi configurationApi)
        {
            _configuration = configuration;
            ConfigurationApi = configurationApi;
        }

        public IConfigurationApi ConfigurationApi { get; }
        public IConfiguration Local => GetConfiguration(SectionTypes.Local);

        public IConfiguration GetConfiguration(SectionTypes sectionType) => _configuration.GetSection(sectionType.ToString());
    }
}