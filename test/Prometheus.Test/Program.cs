using Glasssix.Contrib.Data.Storage.Prometheus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus.Test;

var host = Host.CreateDefaultBuilder()
  .UseConsoleLifetime()
  .ConfigureServices(services =>
  {
      services.AddPrometheusClient("http://10.168.1.47:32196");
      services.AddSingleton<SampleService>();
  })
  .Build();
host.Services.GetRequiredService<SampleService>().QueryAsync().GetAwaiter().GetResult();
host.Run();