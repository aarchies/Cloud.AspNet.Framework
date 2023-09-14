using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.KeyPerFile;
using Microsoft.Extensions.Configuration.Memory;
using System;
using System.Collections.Generic;

namespace Glasssix.BuildingBlocks.Configuration.Internal
{
    internal static class ConfigurationExtensions
    {
        public static readonly List<Type> DefaultExcludeConfigurationProviderTypes = new()
    {
        typeof(EnvironmentVariablesConfigurationProvider),
        typeof(MemoryConfigurationProvider),
        typeof(CommandLineConfigurationProvider),
        typeof(KeyPerFileConfigurationProvider)
    };

        public static readonly List<Type> DefaultExcludeConfigurationSourceTypes = new()
    {
        typeof(CommandLineConfigurationSource),
        typeof(EnvironmentVariablesConfigurationSource),
        typeof(KeyPerFileConfigurationSource),
        typeof(MemoryConfigurationSource)
    };

        public static IConfigurationBuilder AddRange(this IConfigurationBuilder configurationBuilder,
            IEnumerable<IConfigurationSource> configurationSources)
        {
            foreach (var configurationSource in configurationSources)
                configurationBuilder.Add(configurationSource);
            return configurationBuilder;
        }
    }
}