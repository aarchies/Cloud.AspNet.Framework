using Glasssix.Contrib.Data.Storage.InfluxDb.Abstractions;
using Glasssix.Contrib.Data.Storage.InfluxDb.InfluxDb;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Glasssix.Contrib.Data.Storage
{
    /// <summary>
    /// 数据库持久化拓展
    /// </summary>
    public static class StorageDependencies
    {

        /// <summary>
        /// Elasticsearch支持
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IServiceCollection AddElasticsearch(this IServiceCollection service)
        {
            return service;
        }

        /// <summary>
        /// Influx支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddInflux(this IServiceCollection services, Action<InfluxConfig> config)
        {
            services.AddOptions().Configure<InfluxConfig>(config);
            services.AddScoped<IStorage, InfluxDbStorage>();
            return services;
        }
    }
}