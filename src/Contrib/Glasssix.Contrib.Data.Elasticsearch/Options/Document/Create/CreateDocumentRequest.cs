using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Create
{
    public class CreateDocumentRequest<TDocument> : DocumentOptions where TDocument : class
    {
        public CreateDocumentRequest(string indexName, TDocument document, string? documentId) : base(indexName)
            => Request = new SingleDocumentBaseRequest<TDocument>(document, documentId);

        public SingleDocumentBaseRequest<TDocument> Request { get; }
    }
}