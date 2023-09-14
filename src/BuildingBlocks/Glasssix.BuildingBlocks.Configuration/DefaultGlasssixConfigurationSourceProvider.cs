using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Glasssix.BuildingBlocks.Configuration
{
    public class DefaultGlasssixConfigurationSourceProvider : IGlasssixConfigurationSourceProvider
    {
        public virtual (List<IConfigurationSource> MigrateConfigurationSources, List<IConfigurationSource> ConfigurationSources) GetMigrated(
            IEnumerable<IConfigurationSource> originalConfigurationSources,
            List<Type> excludeConfigurationSourceTypes,
            List<Type> excludeConfigurationProviderTypes)
        {
            List<IConfigurationSource> migrateConfigurationSources = new();
            List<IConfigurationSource> configurationSources = new();
            foreach (var originalConfigurationSource in originalConfigurationSources)
            {
                var result = GetMigrated(originalConfigurationSource, excludeConfigurationSourceTypes, excludeConfigurationProviderTypes);
                migrateConfigurationSources.AddRange(result.MigrateConfigurationSources);
                configurationSources.AddRange(result.ConfigurationSources);
            }
            return (migrateConfigurationSources, configurationSources);
        }

        public virtual (List<IConfigurationSource> MigrateConfigurationSources, List<IConfigurationSource> ConfigurationSources)
            GetMigrated(
                IConfiguration configuration,
                List<Type> excludeConfigurationSourceTypes,
                List<Type> excludeConfigurationProviderTypes)
        {
            List<IConfigurationSource> migrateConfigurationSources = new();
            List<IConfigurationSource> configurationSources = new();
            if (configuration is IConfigurationBuilder configurationBuilder)
            {
                foreach (var configurationSource in configurationBuilder.Sources)
                {
                    var result = GetMigrated(
                        configurationSource,
                        excludeConfigurationSourceTypes,
                        excludeConfigurationProviderTypes);
                    migrateConfigurationSources.AddRange(result.MigrateConfigurationSources);
                    configurationSources.AddRange(result.ConfigurationSources);
                }
            }
            else if (configuration is IConfigurationRoot configurationRoot)
            {
                foreach (var configurationProvider in configurationRoot.Providers)
                {
                    var GlasssixConfigurationSource = new GlasssixConfigurationSource(configurationProvider);
                    if (excludeConfigurationProviderTypes.Contains(configurationProvider.GetType()))
                        configurationSources.Add(GlasssixConfigurationSource);
                    else migrateConfigurationSources.Add(GlasssixConfigurationSource);
                }
            }
            return new(migrateConfigurationSources, configurationSources);
        }

        public virtual (List<IConfigurationSource> MigrateConfigurationSources, List<IConfigurationSource> ConfigurationSources)
            GetMigrated(
                IConfigurationSource configurationSource,
                List<Type> excludeConfigurationSourceTypes,
                List<Type> excludeConfigurationProviderTypes)
        {
            List<IConfigurationSource> migrateConfigurationSources = new();
            List<IConfigurationSource> configurationSources = new();
            if (excludeConfigurationSourceTypes.Contains(configurationSource.GetType()))
            {
                configurationSources.Add(configurationSource);
                return (migrateConfigurationSources, configurationSources);
            }
            if (configurationSource is ChainedConfigurationSource chainedConfigurationSource)
                return GetMigrated(
                    chainedConfigurationSource.Configuration,
                    excludeConfigurationSourceTypes,
                    excludeConfigurationProviderTypes);

            migrateConfigurationSources.Add(configurationSource);
            return (migrateConfigurationSources, configurationSources);
        }
    }
}