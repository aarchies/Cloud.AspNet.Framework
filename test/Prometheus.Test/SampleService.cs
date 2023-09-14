using Glasssix.Contrib.Data.Storage.Prometheus;
using Glasssix.Contrib.Data.Storage.Prometheus.Enums;
using Glasssix.Contrib.Data.Storage.Prometheus.Model.Reponse.Query;
using Glasssix.Contrib.Data.Storage.Prometheus.Model.Request;

namespace Prometheus.Test
{
    public class SampleService
    {

        private IGlasssixPrometheusClient _client;

        public SampleService(IGlasssixPrometheusClient client)
        {
            _client = client;
        }

        public async Task QueryAsync()
        {
            var query = new QueryRequest
            {
                Query = "up", //metric name
                Time = "2022-06-01T09:00:00.000Z" //标准时间格式或unix时间戳，如：1654045200或1654045200.000
            };
            var result = await _client.QueryAsync(query);
            if (result.Status == ResultStatuses.Success)
            {
                switch (result.Data.ResultType)
                {
                    case ResultTypes.Vector:
                        {
                            var data = result.Data.Result as QueryResultInstantVectorResponse[];
                            
                    }
                        break;
                    case ResultTypes.Matrix:
                        {
                            var data = result.Data.Result as QueryResultMatrixRangeResponse[];
                            
                    }
                        break;
                    default:
                        {
                            var timeSpan = (double)result.Data.Result[0];
                            var value = (string)result.Data.Result[1];
                        }
                        break;
                }
            }
        }
    }

}
