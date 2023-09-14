using Glasssix.BuildingBlocks.Logging.Extensions;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Glasssix.BuildingBlocks.Logging.Logger
{
    public static class LoggerServiceProvider
    {
        public static IWebHostBuilder UseSerilogDefault(this IWebHostBuilder builder)
        {
            
            builder.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
                string WriteToName = context.Configuration.GetSection("Serilog:WriteTo:0:Name")?.Value;
                if (string.IsNullOrWhiteSpace(WriteToName) || WriteToName != "Elasticsearch")
                {
                    //config.MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                    //.MinimumLevel.Override("System", LogEventLevel.Warning)
                    //.MinimumLevel.Override("Default", LogEventLevel.Debug)
                    //.WriteTo.Console();
                }
                if (WriteToName == "Elasticsearch")
                {
                    var esAgr = context.Configuration.GetSection("Serilog:WriteTo:0:Args");
                    string esUri = esAgr.GetSection("Uris").Value;
                    string indexFormat = esAgr.GetSection("IndexFormat").Value;
                    string userName = esAgr.GetSection("UserName").Value;
                    string password = esAgr.GetSection("Password").Value;

                    config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                   .MinimumLevel.Override("System", LogEventLevel.Warning)
                   .MinimumLevel.Information()
                   .Enrich.FromLogContext()
                   .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(esUri))
                   {
                       IndexFormat = indexFormat,
                       EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
                       FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                       AutoRegisterTemplate = true,
                       AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                       ModifyConnectionSettings =
                           conn =>
                           {
                               conn.ServerCertificateValidationCallback((source, certificate, chain, sslPolicyErrors) => true);
                               conn.BasicAuthentication(userName, password);
                               return conn;
                           }
                   })
                   .WriteTo.Console();
                }
            });
            InitLog.InitializationLog();

            return builder;
        }
    }
}