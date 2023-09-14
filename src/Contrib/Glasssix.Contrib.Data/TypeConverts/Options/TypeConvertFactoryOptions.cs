using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.TypeConverts.Interfaces;
using System;
using System.Linq;

namespace Glasssix.Contrib.Data.TypeConverts.Options
{
    public class TypeConvertFactoryOptions : GlasssixFactoryOptions<TypeConvertRelationOptions>
    {
        public Func<IServiceProvider, ITypeConvertProvider>? GetTypeConvert()
            => GetTypeConvert(Microsoft.Extensions.Options.Options.DefaultName);

        public Func<IServiceProvider, ITypeConvertProvider>? GetTypeConvert(string name)
        {
            return Options.FirstOrDefault(opt => opt.Name == name.ToLower())?.Func;
        }

        public TypeConvertFactoryOptions Mapping(Func<IServiceProvider, ITypeConvertProvider> func)
            => Mapping(Microsoft.Extensions.Options.Options.DefaultName, func);

        public TypeConvertFactoryOptions Mapping(string name, Func<IServiceProvider, ITypeConvertProvider> func)
        {
            var builder = Options.FirstOrDefault(opt => opt.Name == name.ToLower());
            if (builder != null)
            {
                builder.Func = func;
            }
            else
            {
                Options.Add(new TypeConvertRelationOptions(name.ToLower(), func));
            }
            return this;
        }

        public TypeConvertFactoryOptions TryMapping(Func<IServiceProvider, ITypeConvertProvider> func)
                            => TryMapping(Microsoft.Extensions.Options.Options.DefaultName, func);

        public TypeConvertFactoryOptions TryMapping(string name, Func<IServiceProvider, ITypeConvertProvider> func)
        {
            if (Options.Any(opt => opt.Name == name.ToLower()))
                return this;

            Options.Add(new TypeConvertRelationOptions(name.ToLower(), func));
            return this;
        }
    }
}