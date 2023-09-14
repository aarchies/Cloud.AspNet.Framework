using Glasssix.Contrib.Data.Elasticsearch.Options.Document;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Count
{
    public class CountDocumentRequest : DocumentOptions
    {
        public CountDocumentRequest(string indexNameOrAlias) : base(indexNameOrAlias)
        {
        }
    }
}