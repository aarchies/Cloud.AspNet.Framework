using Glasssix.BuildingBlocks.Grpc.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Glasssix.DotNet.Framework.Grpc
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 注册Grpc服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder MapGrpcServices(this IApplicationBuilder builder)
        {
            var entries = new GrpcServerRegistrator(builder.ApplicationServices.GetRequiredService<ILogger<GrpcServerRegistrator>>()).GetEntries();
            foreach (var entry in entries)
            {
                var grpcDelegate =
                    (IMapGrpcProxy)Activator.CreateInstance(typeof(MapGrpcProxy<>).MakeGenericType(entry))!;
                grpcDelegate.Map(builder);
            }
            return builder;
        }

        /// <summary>
        /// 注册Grpc服务
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGrpcServer(this IApplicationBuilder app)
        {
            app.MapGrpcServices();
            return app;
        }
    }
}