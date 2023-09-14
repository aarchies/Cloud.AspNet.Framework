using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Query
{
    public class QueryResultInstantVectorResponse
    {
        public IDictionary<string, object>? Metric { get; set; }

        public object[]? Value { get; set; }
    }
}