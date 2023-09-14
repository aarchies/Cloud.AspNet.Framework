using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Options
{
    public class GlasssixFactoryOptions<TRelationOptions> where TRelationOptions : GlasssixRelationOptions
    {
        public List<TRelationOptions> Options { get; set; } = new List<TRelationOptions>();
    }
}