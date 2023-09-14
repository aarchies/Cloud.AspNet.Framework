namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class GetMultiResponseItems<TDocument>
        where TDocument : class
    {
        public GetMultiResponseItems(string id, TDocument document)
        {
            Id = id;
            Document = document;
        }

        public TDocument Document { get; }
        public string Id { get; }
    }
}