using Glasssix.Contrib.Caller.Options;
using Glasssix.Contrib.Data;
using Glasssix.Contrib.Data.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Caller
{
    internal class DefaultCallerFactory : GlasssixFactoryBase<ICaller, CallerRelationOptions>, ICallerFactory
    {
        private readonly IOptionsMonitor<CallerFactoryOptions> _options;

        public DefaultCallerFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _options = serviceProvider.GetRequiredService<IOptionsMonitor<CallerFactoryOptions>>();
        }

        protected override string DefaultServiceNotFoundMessage => "No default Caller found, you may need service.AddCaller()";

        protected override GlasssixFactoryOptions<CallerRelationOptions> FactoryOptions => _options.CurrentValue;
        protected override string SpecifyServiceNotFoundMessage => "Please make sure you have used [{0}] Caller, it was not found";
    }
}