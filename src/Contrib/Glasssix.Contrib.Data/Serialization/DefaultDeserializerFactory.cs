using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.Serialization.Interfaces;
using Glasssix.Contrib.Data.Serialization.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Data.Serialization
{
    public class DefaultDeserializerFactory : GlasssixFactoryBase<IDeserializer, DeserializerRelationOptions>,
        IDeserializerFactory
    {
        private readonly IOptionsMonitor<DeserializerFactoryOptions> _options;

        public DefaultDeserializerFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _options = serviceProvider.GetRequiredService<IOptionsMonitor<DeserializerFactoryOptions>>();
        }

        protected override string DefaultServiceNotFoundMessage => "Default deserializer not found, you need to add it, like services.AddJson()";

        protected override GlasssixFactoryOptions<DeserializerRelationOptions> FactoryOptions => _options.CurrentValue;
        protected override string SpecifyServiceNotFoundMessage => "Please make sure you have used [{0}] deserializer, it was not found";
    }
}