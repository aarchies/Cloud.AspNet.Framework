using Apm.Api.Test;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(DiagnosticsConfig.ServiceName));
builder.Services.AddOpenTelemetry()
.WithTracing(tracerProviderBuilder =>
    tracerProviderBuilder
      .AddHttpClientInstrumentation()
      .AddSource(DiagnosticsConfig.ActivitySource.Name)
      .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
      .AddAspNetCoreInstrumentation()
      .AddOtlpExporter(opt =>
      {
          opt.Endpoint = new Uri("http://localhost:4317");
          opt.Protocol = OtlpExportProtocol.Grpc;
      })
      .AddConsoleExporter()
      )
.WithMetrics(metricsProviderBuilder =>
    metricsProviderBuilder
      .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
      .AddAspNetCoreInstrumentation()
      .AddOtlpExporter(opt =>
      {
          opt.Endpoint = new Uri("http://localhost:4317");
          opt.Protocol = OtlpExportProtocol.Grpc;
      })
      .AddConsoleExporter()
      .AddPrometheusExporter()
      );


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapGet("/hello", async () =>
{
    var html = await new HttpClient().GetStringAsync("https://example.com/");
    if (string.IsNullOrWhiteSpace(html))
    {
        return "Hello, World!";
    }
    else
    {
        return "Hello, World!";
    }
});
app.UseAuthorization();

app.MapControllers();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();

this.tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddAspNetInstrumentation(
        (options) => options.Filter =
            (httpContext) =>
            {
                // only collect telemetry about HTTP GET requests
                return httpContext.Request.HttpMethod.Equals("GET");
            })
    .Build();