namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Update
{
    public class UpdateDocumentBaseRequest<TDocument> where TDocument : class
    {
        public UpdateDocumentBaseRequest(TDocument document, string? documentId = null)
        {
            Document = document;
            DocumentId = documentId;
        }

        public UpdateDocumentBaseRequest(object partialDocument, string? documentId = null)
        {
            PartialDocument = partialDocument;
            DocumentId = documentId;
        }

        public TDocument? Document { get; }

        public string? DocumentId { get; }
        public object? PartialDocument { get; }
    }
}