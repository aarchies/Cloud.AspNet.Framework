using Glasssix.Contrib.Caching.TypeAlias.Options;
using Glasssix.Contrib.Data;
using Glasssix.Contrib.Data.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Glasssix.Contrib.Caching.TypeAlias
{
    public class DefaultTypeAliasFactory : GlasssixFactoryBase<ITypeAliasProvider, TypeAliasRelationOptions>, ITypeAliasFactory
    {
        private readonly IOptionsMonitor<TypeAliasFactoryOptions> _options;

        public DefaultTypeAliasFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _options = serviceProvider.GetRequiredService<IOptionsMonitor<TypeAliasFactoryOptions>>();
        }

        protected override string DefaultServiceNotFoundMessage => "未找到默认TypeAlias";

        protected override GlasssixFactoryOptions<TypeAliasRelationOptions> FactoryOptions => _options.CurrentValue;
        protected override string SpecifyServiceNotFoundMessage => "确保您已使用了TypeAlias";
    }
}