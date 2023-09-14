using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.MetaData
{
    public class MetaResultResponse : ResultBaseResponse
    {
        public Dictionary<string, MetaItemValueModel[]> Data { get; set; }
    }
}