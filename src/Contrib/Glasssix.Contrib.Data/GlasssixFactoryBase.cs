using Glasssix.Contrib.Data.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data
{
    public abstract class GlasssixFactoryBase<TService, TRelationOptions> : IGlasssixFactory<TService>
        where TService : class
        where TRelationOptions : GlasssixRelationOptions<TService>
    {
        protected readonly IServiceProvider ServiceProvider;

        protected GlasssixFactoryBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected abstract string DefaultServiceNotFoundMessage { get; }
        protected abstract GlasssixFactoryOptions<TRelationOptions> FactoryOptions { get; }
        protected abstract string SpecifyServiceNotFoundMessage { get; }

        public virtual TService Create()
        {
            var defaultOptions = GetDefaultOptions(FactoryOptions.Options);
            if (defaultOptions == null)
                throw new NotSupportedException(DefaultServiceNotFoundMessage);

            return defaultOptions.Func!.Invoke(ServiceProvider);
        }

        public virtual TService Create(string name)
        {
            var options = FactoryOptions.Options.SingleOrDefault(c => c.Name!.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (options == null)
                throw new NotSupportedException(string.Format(SpecifyServiceNotFoundMessage, name));

            return options.Func!.Invoke(ServiceProvider);
        }

        private static GlasssixRelationOptions<TService>? GetDefaultOptions(List<TRelationOptions> optionsList)
        {
            return optionsList.SingleOrDefault(c => c.Name == Microsoft.Extensions.Options.Options.DefaultName) ??
                optionsList.FirstOrDefault();
        }
    }
}