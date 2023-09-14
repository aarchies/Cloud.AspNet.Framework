using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Delete
{
    public class DeleteDocumentRequest : DocumentOptions
    {
        public DeleteDocumentRequest(string indexName, string documentId) : base(indexName) => DocumentId = documentId;

        public string DocumentId { get; }
    }
}