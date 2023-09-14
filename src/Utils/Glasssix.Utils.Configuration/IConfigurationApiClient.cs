using System;
using System.Threading.Tasks;

namespace Glasssix.Utils.Configuration
{
    public interface IConfigurationApiClient
    {
        Task<T> GetAsync<T>(string configObject, Action<T> valueChanged = null);

        Task<T> GetAsync<T>(string environment, string cluster, string appId, string configObject, Action<T> valueChanged = null);

        Task<dynamic> GetDynamicAsync(string environment, string cluster, string appId, string configObject, Action<dynamic> valueChanged = null);

        Task<dynamic> GetDynamicAsync(string configObject);

        Task<(string Raw, ConfigurationTypes ConfigurationType)> GetRawAsync(string configObject, Action<string> valueChanged = null);

        Task<(string Raw, ConfigurationTypes ConfigurationType)> GetRawAsync(string environment, string cluster, string appId, string configObject, Action<string> valueChanged = null);
    }
}