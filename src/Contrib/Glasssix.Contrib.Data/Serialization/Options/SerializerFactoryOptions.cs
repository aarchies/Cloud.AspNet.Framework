using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.Serialization.Interfaces;
using System;
using System.Linq;

namespace Glasssix.Contrib.Data.Serialization.Options
{
    public class SerializerFactoryOptions : GlasssixFactoryOptions<SerializerRelationOptions>
    {
        public Func<IServiceProvider, ISerializer>? GetSerializer()
            => GetSerializer(Microsoft.Extensions.Options.Options.DefaultName);

        public Func<IServiceProvider, ISerializer>? GetSerializer(string name)
        {
            var serializer = Options.FirstOrDefault(b => b.Name == name.ToLower());
            return serializer?.Func;
        }

        public SerializerFactoryOptions MappingSerializer(string name, Func<IServiceProvider, ISerializer> func)
        {
            var builder = Options.FirstOrDefault(b => b.Name == name.ToLower());
            if (builder != null) builder.Func = func;
            else Options.Add(new SerializerRelationOptions(name.ToLower(), func));
            return this;
        }
    }
}