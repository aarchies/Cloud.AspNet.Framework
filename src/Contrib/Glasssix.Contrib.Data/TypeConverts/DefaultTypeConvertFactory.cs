using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.TypeConverts.Interfaces;
using Glasssix.Contrib.Data.TypeConverts.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Data.TypeConverts
{
    public class DefaultTypeConvertFactory : GlasssixFactoryBase<ITypeConvertProvider, TypeConvertRelationOptions>,
        ITypeConvertFactory
    {
        private readonly IOptionsMonitor<TypeConvertFactoryOptions> _options;

        public DefaultTypeConvertFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _options = serviceProvider.GetRequiredService<IOptionsMonitor<TypeConvertFactoryOptions>>();
        }

        protected override string DefaultServiceNotFoundMessage
                    => "Default typeConvert not found, you need to add it, like services.AddTypeConvert()";

        protected override GlasssixFactoryOptions<TypeConvertRelationOptions> FactoryOptions => _options.CurrentValue;
        protected override string SpecifyServiceNotFoundMessage => "Please make sure you have used [{0}] typeConvert, it was not found";
    }
}