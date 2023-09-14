using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public class DefaultElasticsearchFactory : IElasticsearchFactory
    {
        private readonly IElasticClientFactory _factory;

        public DefaultElasticsearchFactory(IElasticClientFactory factory)
            => _factory = factory;

        public IGlasssixElasticClient CreateClient() => new DefaultGlasssixElasticClient(CreateElasticClient());

        public IGlasssixElasticClient CreateClient(string name) => new DefaultGlasssixElasticClient(CreateElasticClient(name));

        public IElasticClient CreateElasticClient() => _factory.Create();

        public IElasticClient CreateElasticClient(string name) => _factory.Create(name);
    }
}