using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public class GlasssixElasticsearchBuilder
    {
        private IElasticClient? _elasticClient;
        private IElasticClientFactory? _elasticClientFactory;

        public GlasssixElasticsearchBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public IGlasssixElasticClient Client => new DefaultGlasssixElasticClient(ElasticClient);

        public IElasticClient ElasticClient
        {
            get
            {
                if (_elasticClient == null)
                {
                    _elasticClientFactory ??= Services.BuildServiceProvider().GetRequiredService<IElasticClientFactory>();
                    _elasticClient = _elasticClientFactory.Create(Name);
                }

                return _elasticClient;
            }
        }

        public string Name { get; }
        public IServiceCollection Services { get; }
    }
}