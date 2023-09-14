namespace Glasssix.Contrib.Data.Elasticsearch
{
    public interface IGlasssixElasticClientFactory
    {
        IGlasssixElasticClient Create();

        IGlasssixElasticClient Create(string name);
    }
}