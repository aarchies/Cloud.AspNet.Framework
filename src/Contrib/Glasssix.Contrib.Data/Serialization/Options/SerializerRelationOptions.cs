using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.Serialization.Interfaces;
using System;

namespace Glasssix.Contrib.Data.Serialization.Options
{
    public class SerializerRelationOptions : GlasssixRelationOptions<ISerializer>
    {
        public SerializerRelationOptions(string name, Func<IServiceProvider, ISerializer> func)
            : base(name)
        {
            Func = func;
        }
    }
}