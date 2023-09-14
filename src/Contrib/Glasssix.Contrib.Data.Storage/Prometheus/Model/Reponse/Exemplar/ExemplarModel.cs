using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Exemplar
{
    public class ExemplarDataModel
    {
        public IEnumerable<ExemplarModel>? Exemplars { get; set; }
        public IDictionary<string, object>? SeriesLabels { get; set; }
    }
}