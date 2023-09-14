namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.MetaData
{
    public class MetricMetaQueryRequest
    {
        /// <summary>
        /// default all ,if set 0, then can't return any data
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// search metric name ,use full match
        /// </summary>
        public string Metric { get; set; }
    }
}