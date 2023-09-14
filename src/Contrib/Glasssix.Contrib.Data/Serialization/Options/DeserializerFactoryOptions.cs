using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.Serialization.Interfaces;
using System;
using System.Linq;

namespace Glasssix.Contrib.Data.Serialization.Options
{
    public class DeserializerFactoryOptions : GlasssixFactoryOptions<DeserializerRelationOptions>
    {
        public Func<IServiceProvider, IDeserializer>? GetDeserializer()
            => GetDeserializer(Microsoft.Extensions.Options.Options.DefaultName);

        public Func<IServiceProvider, IDeserializer>? GetDeserializer(string name)
        {
            var deserializer = Options.FirstOrDefault(b => b.Name == name.ToLower());
            return deserializer?.Func;
        }

        public DeserializerFactoryOptions MappingDeserializer(string name, Func<IServiceProvider, IDeserializer> func)
        {
            var builder = Options.FirstOrDefault(b => b.Name == name.ToLower());
            if (builder != null) builder.Func = func;
            else Options.Add(new DeserializerRelationOptions(name.ToLower(), func));
            return this;
        }
    }
}