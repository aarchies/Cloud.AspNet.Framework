using Glasssix.Contrib.Data.Storage.Prometheus.Enums;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Query
{
    public class QueryResultDataResponse
    {
        public object[]? Result { get; set; }
        public ResultTypes ResultType { get; set; }
    }
}