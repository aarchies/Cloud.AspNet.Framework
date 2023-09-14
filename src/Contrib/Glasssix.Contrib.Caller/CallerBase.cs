using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Caller
{
    public abstract class CallerBase
    {
        private ICaller? _caller;

        protected CallerBase() => ServiceProvider = null;

        protected CallerBase(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;

        public virtual string? Name { get; private set; } = null;

        public IServiceProvider? ServiceProvider { get; private set; }
        protected ICaller Caller => _caller ??= ServiceProvider!.GetRequiredService<ICallerFactory>().Create(Name!);
        protected CallerOptionsBuilder CallerOptions { get; private set; } = default!;

        [Obsolete("CallerProvider has expired, please use Caller")]
        protected ICaller CallerProvider => Caller;

        public void SetCallerOptions(CallerOptionsBuilder callerOptionsBuilder, string name)
        {
            CallerOptions = callerOptionsBuilder;
            Name = name;
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public abstract void UseCallerExtension();

        protected virtual Task ConfigHttpRequestMessageAsync(HttpRequestMessage requestMessage)
        {
            return Task.CompletedTask;
        }
    }
}