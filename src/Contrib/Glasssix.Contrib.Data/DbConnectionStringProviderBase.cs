using Glasssix.Contrib.Data.Options;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data
{
    public abstract class BaseDbConnectionStringProvider : DbConnectionStringProviderBase
    {
    }

    public abstract class DbConnectionStringProviderBase : IDbConnectionStringProvider
    {
        private readonly List<GlasssixDbContextConfigurationOptions>? _dbContextOptionsList = null;

        public virtual List<GlasssixDbContextConfigurationOptions> DbContextOptionsList => _dbContextOptionsList ?? GetDbContextOptionsList();

        protected abstract List<GlasssixDbContextConfigurationOptions> GetDbContextOptionsList();
    }
}