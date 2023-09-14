using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Glasssix.Contrib.Data.Orm.SqlSugar
{
    public sealed class ConfigHelper
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> _configurationCache;

        static ConfigHelper()
        {
            _configurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
        }
    }
}