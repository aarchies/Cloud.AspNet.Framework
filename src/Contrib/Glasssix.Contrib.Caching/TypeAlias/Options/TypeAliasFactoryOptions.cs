using Glasssix.Contrib.Data.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Glasssix.Contrib.Caching.TypeAlias.Options
{
    /// <summary>
    /// 别名工厂选项
    /// </summary>
    public class TypeAliasFactoryOptions : GlasssixFactoryOptions<TypeAliasRelationOptions>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        public void TryAdd(string name)
        {
            if (Options.Any(options => options.Name == name))
                return;

            var typeAliasRelationOptions = new TypeAliasRelationOptions(
                name,
                serviceProvider
                    => new DefaultTypeAliasProvider(serviceProvider.GetService<IOptionsFactory<TypeAliasOptions>>()?.Create(name))
            );
            Options.Add(typeAliasRelationOptions);
        }
    }
}