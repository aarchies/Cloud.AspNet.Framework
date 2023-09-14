namespace Glasssix.Contrib.Data.Elasticsearch
{
    public class DefaultGlasssixElasticClientFactory : IGlasssixElasticClientFactory
    {
        private readonly IElasticClientFactory _clientFactory;

        public DefaultGlasssixElasticClientFactory(IElasticClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IGlasssixElasticClient Create()
        {
            var elasticClient = _clientFactory.Create();
            return new DefaultGlasssixElasticClient(elasticClient);
        }

        public IGlasssixElasticClient Create(string name)
        {
            var elasticClient = _clientFactory.Create(name);
            return new DefaultGlasssixElasticClient(elasticClient);
        }
    }
}