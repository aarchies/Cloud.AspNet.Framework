using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Exemplar
{
    public class ExemplarResultResponse : ResultBaseResponse
    {
        public IEnumerable<ExemplarDataModel>? Data { get; set; }
    }
}