using Microsoft.Extensions.Configuration;

namespace Glasssix.BuildingBlocks.Configuration
{
    public class GlasssixConfigurationSource : IConfigurationSource
    {
        internal readonly GlasssixConfigurationBuilder? Builder;

        internal readonly IConfigurationProvider? ConfigurationProvider;

        public GlasssixConfigurationSource(GlasssixConfigurationBuilder builder) => Builder = builder;

        public GlasssixConfigurationSource(IConfigurationProvider configurationProvider) => ConfigurationProvider = configurationProvider;

        public IConfigurationProvider Build(IConfigurationBuilder builder)
            => Builder != null ? new GlasssixConfigurationProvider(this) : ConfigurationProvider!;
    }
}