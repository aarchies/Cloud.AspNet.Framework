using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller
{
    public interface IResponseMessage
    {
        Task<TResponse?> ProcessResponseAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken = default);

        Task ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken = default);
    }
}