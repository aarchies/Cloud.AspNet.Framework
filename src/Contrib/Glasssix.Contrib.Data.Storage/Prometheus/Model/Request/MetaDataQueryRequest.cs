using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Request
{
    public class MetaDataQueryRequest
    {
        public string? End { get; set; }
        public IEnumerable<string>? Match { get; set; }

        public string? Start { get; set; }
    }
}