namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class BulkResponseItems
    {
        public BulkResponseItems(string id, bool isValid, string message)
        {
            Id = id;
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// The id of the document for the bulk operation
        /// </summary>
        public string Id { get; }

        public bool IsValid { get; }

        public string Message { get; }
    }
}