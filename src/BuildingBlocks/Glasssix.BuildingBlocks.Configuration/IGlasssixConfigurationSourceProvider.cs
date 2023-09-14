using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Glasssix.BuildingBlocks.Configuration
{
    public interface IGlasssixConfigurationSourceProvider
    {
        (List<IConfigurationSource> MigrateConfigurationSources, List<IConfigurationSource> ConfigurationSources) GetMigrated(
            IEnumerable<IConfigurationSource> originalConfigurationSources,
            List<Type> excludeConfigurationSourceTypes,
            List<Type> excludeConfigurationProviderTypes);
    }
}