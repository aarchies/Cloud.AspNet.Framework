namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Request
{
    public class QueryRangeRequest
    {
        public string? End { get; set; }
        public string? Query { get; set; }

        public string? Start { get; set; }
        public string? Step { get; set; }

        public string? TimeOut { get; set; }
    }
}