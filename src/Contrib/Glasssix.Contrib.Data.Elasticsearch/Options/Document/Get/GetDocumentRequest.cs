using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Get
{
    public class GetDocumentRequest : DocumentOptions
    {
        public GetDocumentRequest(string indexName, string id) : base(indexName) => Id = id;

        public string Id { get; }
    }
}