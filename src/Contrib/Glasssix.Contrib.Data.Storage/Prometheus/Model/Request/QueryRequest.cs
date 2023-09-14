namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Request
{
    public class QueryRequest
    {
        public string? Query { get; set; }

        public string? Time { get; set; }

        public string? TimeOut { get; set; }
    }
}