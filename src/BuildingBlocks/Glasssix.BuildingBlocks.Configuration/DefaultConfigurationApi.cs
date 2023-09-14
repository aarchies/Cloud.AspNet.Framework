using Glasssix.Utils.Configuration;
using Microsoft.Extensions.Configuration;

namespace Glasssix.BuildingBlocks.Configuration
{
    public class DefaultConfigurationApi : IConfigurationApi
    {
        private readonly IConfiguration _configuration;

        public DefaultConfigurationApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Get(string appId)
        {
            return _configuration.GetSection(SectionTypes.ConfigurationApi.ToString()).GetSection(appId);
        }
    }
}