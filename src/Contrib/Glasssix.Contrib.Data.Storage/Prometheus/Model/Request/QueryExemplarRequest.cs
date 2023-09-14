namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Request
{
    public class QueryExemplarRequest
    {
        public string? End { get; set; }
        public string? Query { get; set; }

        public string? Start { get; set; }
    }
}