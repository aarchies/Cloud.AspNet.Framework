using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Index
{
    public class CreateIndexOptions
    {
        public IAliases? Aliases { get; set; } = null;
        public IIndexSettings? IndexSettings { get; set; } = null;
        public ITypeMapping? Mappings { get; set; } = null;
    }
}