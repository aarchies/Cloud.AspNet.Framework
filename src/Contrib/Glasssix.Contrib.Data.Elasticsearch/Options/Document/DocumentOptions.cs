using System;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document
{
    public class DocumentOptions
    {
        public DocumentOptions(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentException("indexName cannot be empty", nameof(indexName));

            IndexName = indexName;
        }

        public string IndexName { get; }
    }
}