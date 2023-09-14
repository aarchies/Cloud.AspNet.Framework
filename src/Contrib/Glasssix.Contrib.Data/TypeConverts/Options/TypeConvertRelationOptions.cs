using Glasssix.Contrib.Data.Options;
using Glasssix.Contrib.Data.TypeConverts.Interfaces;
using System;

namespace Glasssix.Contrib.Data.TypeConverts.Options
{
    public class TypeConvertRelationOptions : GlasssixRelationOptions<ITypeConvertProvider>
    {
        public TypeConvertRelationOptions(string name, Func<IServiceProvider, ITypeConvertProvider> func)
            : base(name)
        {
            Func = func;
        }
    }
}