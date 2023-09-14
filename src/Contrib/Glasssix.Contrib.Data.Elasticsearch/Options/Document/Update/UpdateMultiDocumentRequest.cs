using System.Collections.Generic;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Update
{
    public class UpdateMultiDocumentRequest<TDocument> : DocumentOptions where TDocument : class
    {
        public UpdateMultiDocumentRequest(string indexName) : base(indexName)
            => Items = new();

        public List<UpdateDocumentBaseRequest<TDocument>> Items { get; set; }

        public UpdateMultiDocumentRequest<TDocument> AddDocument(UpdateDocumentBaseRequest<TDocument> item)
        {
            Items.Add(item);
            return this;
        }
    }
}