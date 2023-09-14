using Glasssix.BuildingBlocks.Configuration.Extensions;
using Glasssix.Utils.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Glasssix.BuildingBlocks.Configuration
{
    internal class LocalGlasssixConfigurationRepository : AbstractConfigurationRepository
    {
        private Properties _data = new();

        public LocalGlasssixConfigurationRepository(IConfiguration configuration)
        {
            Initialize(configuration);

            ChangeToken.OnChange(configuration.GetReloadToken, () =>
            {
                Initialize(configuration);
                FireRepositoryChange(SectionType, Load());
            });
        }

        public override SectionTypes SectionType => SectionTypes.Local;

        public override Properties Load()
        {
            return _data;
        }

        private void Initialize(IConfiguration configuration)
        {
            var data = configuration.ConvertToDictionary();
            _data = new Properties(data);
        }
    }
}