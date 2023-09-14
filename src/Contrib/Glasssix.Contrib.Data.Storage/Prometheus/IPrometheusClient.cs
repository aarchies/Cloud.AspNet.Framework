using Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Exemplar;
using Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.MetaData;
using Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Query;
using Glasssix.Contrib.Data.Storage.Prometheus.Model.Request;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Storage.Prometheus
{
    public interface IGlasssixPrometheusClient
    {
        Task<ExemplarResultResponse> ExemplarQueryAsync(QueryExemplarRequest query);

        Task<LabelResultResponse> LabelsQueryAsync(MetaDataQueryRequest query);

        Task<LabelResultResponse> LabelValuesQueryAsync(LableValueQueryRequest query);

        Task<MetaResultResponse> MetricMetaQueryAsync(MetricMetaQueryRequest query);

        Task<QueryResultCommonResponse> QueryAsync(QueryRequest query);

        Task<QueryResultCommonResponse> QueryRangeAsync(QueryRangeRequest query);

        Task<SeriesResultResponse> SeriesQueryAsync(MetaDataQueryRequest query);
    }
}