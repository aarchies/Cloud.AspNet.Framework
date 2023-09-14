using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public interface IElasticClientFactory
    {
        IElasticClient Create();

        IElasticClient Create(string name);
    }
}