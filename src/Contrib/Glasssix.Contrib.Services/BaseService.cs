using Glasssix.Contrib.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Glasssix.Contrib.Services
{
    /// <summary>
    /// 应用服务
    /// </summary>
    public abstract class BaseService : IService
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger _log;
    }
}