using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch.Options.Document.Query
{
    public class QueryOptions<TDocument> : QueryBaseOptions<TDocument>
        where TDocument : class
    {
        public QueryOptions(string indexName, string query, string defaultField, int skip, int take, Operator @operator = Operator.Or)
            : base(indexName, query, defaultField, @operator)
        {
            Skip = skip;
            Take = take;
        }

        public int Skip { get; }

        public int Take { get; }
    }
}