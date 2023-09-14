using System.Collections.Generic;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Delete
{
    public class DeleteMultiDocumentRequest : DocumentOptions
    {
        public DeleteMultiDocumentRequest(string indexName, params string[] documentIds) : base(indexName)
            => DocumentIds = documentIds;

        public DeleteMultiDocumentRequest(string indexName, IEnumerable<string> documentIds) : base(indexName)
            => DocumentIds = documentIds;

        public IEnumerable<string> DocumentIds { get; }
    }
}