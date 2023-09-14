using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Query
{
    public class QueryResultMatrixRangeResponse
    {
        public IDictionary<string, object>? Metric { get; set; }

        public IEnumerable<object[]>? Values { get; set; }
    }
}