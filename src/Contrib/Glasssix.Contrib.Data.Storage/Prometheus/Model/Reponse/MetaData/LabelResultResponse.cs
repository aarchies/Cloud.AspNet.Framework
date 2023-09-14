using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.MetaData
{
    public class LabelResultResponse : ResultBaseResponse
    {
        public IEnumerable<string>? Data { get; set; }
    }
}