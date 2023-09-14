using System;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data
{
    public interface IDistributedLock
    {
        IDisposable? TryGet(string key, TimeSpan timeout = default);

        Task<IAsyncDisposable?> TryGetAsync(string key, TimeSpan timeout = default, CancellationToken cancellationToken = default);
    }
}