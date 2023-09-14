using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Glasssix.BuildingBlocks.Configuration.Options
{
    public class ConfigurationOptions
    {
        public ConfigurationOptions()
        {
            Assemblies = GlasssixIocApp.GetAssemblies();
            ExcludeConfigurationSourceTypes = Internal.ConfigurationExtensions.DefaultExcludeConfigurationSourceTypes;
            ExcludeConfigurationProviderTypes = Internal.ConfigurationExtensions.DefaultExcludeConfigurationProviderTypes;
        }

        public IEnumerable<Assembly> Assemblies { get; set; }

        public List<Type> ExcludeConfigurationProviderTypes { get; set; }
        public List<Type> ExcludeConfigurationSourceTypes { get; set; }
    }
}