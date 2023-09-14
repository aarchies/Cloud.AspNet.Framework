using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Utils.Configuration
{
    public interface IConfigurationApiManage
    {
        Task AddAsync(string environment, string cluster, string appId, Dictionary<string, string> configObjects, bool isEncryption = false);

        Task UpdateAsync(string environment, string cluster, string appId, string configObject, object value);
    }
}