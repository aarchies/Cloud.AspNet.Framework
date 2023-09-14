using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller
{
    public delegate Task CallerHandlerDelegate();

    public interface ICallerMiddleware
    {
        Task HandleAsync(GlasssixHttpContext GlasssixHttpContext, CallerHandlerDelegate next, CancellationToken cancellationToken = default);
    }
}