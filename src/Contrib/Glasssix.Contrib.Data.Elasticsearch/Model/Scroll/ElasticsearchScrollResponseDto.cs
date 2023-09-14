namespace Glasssix.Contrib.Data.Elasticsearch.Model.Scroll
{
    public class ElasticsearchScrollResponseDto<TResult> : PaginatedListBase<TResult> where TResult : class
    {
        public string ScrollId { get; set; }
    }
}