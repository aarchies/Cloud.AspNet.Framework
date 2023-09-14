using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.BuildingBlocks.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Dictionary<string, string> ConvertToDictionary(this IConfiguration configuration)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            GetData(configuration, configuration.GetChildren(), ref data);
            return data;
        }

        private static void GetData(
            IConfiguration configuration,
            IEnumerable<IConfigurationSection> configurationSections,
            ref Dictionary<string, string> dictionary)
        {
            foreach (var configurationSection in configurationSections)
            {
                var section = configuration.GetSection(configurationSection.Path);

                var childrenSections = section.GetChildren()?.ToList() ?? new List<IConfigurationSection>();

                if (!section.Exists() || !childrenSections.Any())
                {
                    var key = section.Path;
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, configuration[section.Path]);
                    }
                }
                else
                {
                    GetData(configuration, childrenSections, ref dictionary);
                }
            }
        }
    }
}