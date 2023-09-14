using Glasssix.Contrib.Data.Options;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data
{
    public interface IDbConnectionStringProvider
    {
        List<GlasssixDbContextConfigurationOptions> DbContextOptionsList { get; }
    }
}