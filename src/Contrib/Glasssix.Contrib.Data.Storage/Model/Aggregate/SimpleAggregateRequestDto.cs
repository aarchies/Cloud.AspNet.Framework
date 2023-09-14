namespace Glasssix.Contrib.Data.Storage.Model.Aggregate
{
    public class SimpleAggregateRequestDto : BaseRequestDto
    {
        public string Alias { get; set; }

        /// <summary>
        /// currently support elasticsearch: https://www.elastic.co/guide/en/elasticsearch/reference/7.17/search-aggregations-bucket-datehistogram-aggregation.html
        /// </summary>
        public string Interval { get; set; }

        public int MaxCount { get; set; }
        public string Name { get; set; }
        public AggregateTypes Type { get; set; }
    }
}