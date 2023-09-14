using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Set
{
    public class SetDocumentItemBaseRequest<TDocument> : SingleDocumentBaseRequest<TDocument>
        where TDocument : class
    {
        public SetDocumentItemBaseRequest(TDocument document, string? documentId) : base(document, documentId)
        {
        }
    }
}