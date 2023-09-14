using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Exemplar
{
    public class ExemplarModel
    {
        public IDictionary<string, object>? Labels { get; set; }

        public float TimeStamp { get; set; }
        public string? Value { get; set; }
    }
}