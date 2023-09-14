using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Elasticsearch.Options
{
    public class ElasticsearchFactoryOptions
    {
        public List<ElasticsearchRelationsOptions> Options { get; set; } = new();
    }
}