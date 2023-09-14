using System;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document
{
    public class SingleDocumentBaseRequest<TDocument> where TDocument : class
    {
        public SingleDocumentBaseRequest(TDocument document, string? documentId)
        {
            Document = document;
            DocumentId = documentId ?? Guid.NewGuid().ToString();
        }

        public TDocument Document { get; }

        public string? DocumentId { get; }
    }
}