using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.Serialization.Interfaces;
using Glasssix.Contrib.Data.Serialization.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Data.Serialization
{
    public class DefaultSerializerFactory : GlasssixFactoryBase<ISerializer, SerializerRelationOptions>,
        ISerializerFactory
    {
        private readonly IOptionsMonitor<SerializerFactoryOptions> _options;

        public DefaultSerializerFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _options = serviceProvider.GetRequiredService<IOptionsMonitor<SerializerFactoryOptions>>();
        }

        protected override string DefaultServiceNotFoundMessage => "Default serializer not found, you need to add it, like services.AddJson()";

        protected override GlasssixFactoryOptions<SerializerRelationOptions> FactoryOptions => _options.CurrentValue;
        protected override string SpecifyServiceNotFoundMessage => "Please make sure you have used [{0}] serializer, it was not found";
    }
}