using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Alias
{
    public class UnBindAliasIndexOptions
    {
        public UnBindAliasIndexOptions(string alias, string indexName) : this(alias)
        {
            if (string.IsNullOrEmpty(indexName))
                throw new ArgumentException("indexName cannot be empty", nameof(indexName));

            IndexNames = new[] { indexName };
        }

        public UnBindAliasIndexOptions(string alias, IEnumerable<string> indexNames) : this(alias)
        {
            //ArgumentNullException.ThrowIfNull(nameof(indexNames));
            IndexNames = indexNames;
        }

        private UnBindAliasIndexOptions(string alias) => Alias = alias;

        public string Alias { get; }
        public IEnumerable<string> IndexNames { get; } = default!;
    }
}