using Glasssix.Contrib.Data.Options;
using System;

namespace Glasssix.Contrib.Caching.TypeAlias.Options
{
    /// <summary>
    /// 别名关系选项
    /// </summary>
    public class TypeAliasRelationOptions : GlasssixRelationOptions<ITypeAliasProvider>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        public TypeAliasRelationOptions(string name, Func<IServiceProvider, ITypeAliasProvider> func) : base(name)
        {
            Func = func;
        }
    }
}