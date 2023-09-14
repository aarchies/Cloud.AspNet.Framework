using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public interface IElasticsearchFactory
    {

        IGlasssixElasticClient CreateClient();

        IGlasssixElasticClient CreateClient(string name);

        IElasticClient CreateElasticClient();

        IElasticClient CreateElasticClient(string name);
    }
}