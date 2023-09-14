using System;

namespace Glasssix.Contrib.Data.Options
{
    public class GlasssixRelationOptions
    {
        public string? Name { get; protected set; }
    }

    public class GlasssixRelationOptions<TService> : GlasssixRelationOptions
        where TService : class
    {
        public GlasssixRelationOptions(string name) => Name = name;

        public GlasssixRelationOptions(string name, Func<IServiceProvider, TService> func) : this(name)
        {
            Func = func;
        }

        public Func<IServiceProvider, TService>? Func { get; set; }
    }
}