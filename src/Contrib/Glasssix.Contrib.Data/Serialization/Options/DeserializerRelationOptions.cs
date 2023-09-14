using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.Serialization.Interfaces;
using System;

namespace Glasssix.Contrib.Data.Serialization.Options
{
    public class DeserializerRelationOptions : GlasssixRelationOptions<IDeserializer>
    {
        public DeserializerRelationOptions(string name, Func<IServiceProvider, IDeserializer> func)
            : base(name)
        {
            Func = func;
        }
    }
}