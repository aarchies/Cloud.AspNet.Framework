namespace Glasssix.Contrib.Data.Elasticsearch.Model.Scroll
{
    public class ElasticsearchScrollRequestDto : BaseRequestDto
    {
        public string Scroll { get; set; }

        public string ScrollId { get; set; }
    }
}