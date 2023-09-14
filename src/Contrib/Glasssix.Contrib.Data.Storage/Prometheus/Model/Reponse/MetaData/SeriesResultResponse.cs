using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.MetaData
{
    public class SeriesResultResponse : ResultBaseResponse
    {
        public IEnumerable<IDictionary<string, string>>? Data { get; set; }
    }
}