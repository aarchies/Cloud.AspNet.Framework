using Glasssix.Utils.Configuration;
using Glasssix.Utils.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Glasssix.BuildingBlocks.Configuration
{
    public class GlasssixConfigurationBuilder : IGlasssixConfigurationBuilder
    {
        private readonly IConfigurationBuilder _builder;

        private IConfiguration? _configuration;

        public GlasssixConfigurationBuilder(IServiceCollection services, IConfigurationBuilder builder)
        {
            Services = services;
            _builder = builder;
        }

        public IConfiguration Configuration => _configuration ??= _builder.Build();
        public IDictionary<string, object> Properties => _builder.Properties;
        public IServiceCollection Services { get; }
        public IList<IConfigurationSource> Sources => _builder.Sources;
        internal List<ConfigurationRelationOptions> Relations { get; } = new();
        internal List<IConfigurationRepository> Repositories { get; } = new();

        public IConfigurationBuilder Add(IConfigurationSource source) => _builder.Add(source);

        public void AddRelations(params ConfigurationRelationOptions[] relationOptions)
            => Relations.AddRange(relationOptions);

        public void AddRepository(IConfigurationRepository configurationRepository)
                    => Repositories.Add(configurationRepository);

        public IConfigurationRoot Build() => _builder.Build();
    }
}