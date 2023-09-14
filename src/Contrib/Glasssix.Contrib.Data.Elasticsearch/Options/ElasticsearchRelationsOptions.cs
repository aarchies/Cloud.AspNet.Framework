using Nest;
using System;

namespace Glasssix.Contrib.Data.Elasticsearch.Options
{
    public class ElasticsearchRelationsOptions
    {
        public ElasticsearchRelationsOptions(string name)
        {
            Name = name;
        }

        public Func<IServiceProvider, IElasticClient> Func { get; set; }
        public string Name { get; }
    }
}