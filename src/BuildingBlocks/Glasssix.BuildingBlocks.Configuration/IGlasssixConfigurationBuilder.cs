using Glasssix.Utils.Configuration;
using Glasssix.Utils.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.BuildingBlocks.Configuration
{
    public interface IGlasssixConfigurationBuilder : IConfigurationBuilder
    {
        IConfiguration Configuration { get; }
        IServiceCollection Services { get; }

        void AddRelations(params ConfigurationRelationOptions[] relationOptions);

        void AddRepository(IConfigurationRepository configurationRepository);
    }
}