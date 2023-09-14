using System.Collections.Generic;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Get
{
    public class GetMultiDocumentRequest : DocumentOptions
    {
        public GetMultiDocumentRequest(string indexName, params string[] ids) : base(indexName)
            => Ids = ids;

        public GetMultiDocumentRequest(string indexName, IEnumerable<string> ids) : base(indexName)
            => Ids = ids;

        public IEnumerable<string> Ids { get; }
    }
}