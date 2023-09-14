using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Glasssix.Contrib.Data.Storage.Prometheus
{
    public static class ServiceCollectionExtensions
    {
        private const string PROMETHEUS_HTTP_CLIENT_NAME = "Glasssix_stack_prometheus_client";

        public static IServiceCollection AddPrometheusClient(this IServiceCollection services, string url, int timeoutSeconds = 5)
        {

            if (string.IsNullOrEmpty(url))
                new Exception($"UrlÒì³£ {url}");

            if (services.Any(service => service.GetType() == typeof(IGlasssixPrometheusClient)))
                return services;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            if (timeoutSeconds <= 0)
                timeoutSeconds = 5;
            services.AddHttpClient(PROMETHEUS_HTTP_CLIENT_NAME, ops =>
            {
                ops.BaseAddress = new Uri(url);
                ops.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
            });

            services.AddScoped<IGlasssixPrometheusClient>(ServiceProvider =>
            {
                var client = ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(PROMETHEUS_HTTP_CLIENT_NAME);
                return new GlasssixPrometheusClient(client, jsonOptions, ServiceProvider.GetRequiredService<ILogger<GlasssixPrometheusClient>>());
            });
            return services;
        }
    }
}