using Glasssix.Contrib.Data.Storage.InfluxDb.Abstractions;
using InfluxDB.Client;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Storage.InfluxDb.InfluxDb
{
    public class InfluxConfig
    {
        public string Bucket { get; set; }

        public string Org { get; set; }
        public string Server { get; set; }
        public string Token { get; set; }
    }

    public class InfluxDbStorage : IStorage
    {
        private readonly InfluxDBClient Client;
        private readonly InfluxConfig Config;

        public InfluxDbStorage(IOptions<InfluxConfig> options)
        {
            Config = options.Value;
            Client = InfluxDBClientFactory.Create(Config.Server, Config.Token);
        }

        public async Task Apply<T>(T model) where T : class
        {
            var api = Client.GetWriteApiAsync();
            var data = model as PointData;
            await api.WritePointAsync(data);
            //await api.WriteMeasurementsAsync<Decimal>(Config.Bucket, Config.Org, WritePrecision.Ns, model);
        }

        public async Task<List<T>> List<T>()
        {
            var api = Client.GetQueryApi();
            var query = "from(bucket:\"sailing\") |> range(start: 0) |> filter(fn: (r) => r[\"_measurement\"] == \"segment\")";
            var tables = await api.QueryAsync(query, Config.Org);
            return default;
        }
    }
}