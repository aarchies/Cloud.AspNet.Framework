using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Exist
{
    public class ExistDocumentRequest : DocumentOptions
    {
        public ExistDocumentRequest(string indexName, string documentId) : base(indexName)
            => DocumentId = documentId;

        public string DocumentId { get; set; }
    }
}