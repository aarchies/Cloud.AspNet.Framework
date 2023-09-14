using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Glasssix.BuildingBlocks.Grpc.Client
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加GrpcClient
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcClient(this IServiceCollection services)
        {
            services.AddSingleton<ClientLoggerInterceptor>();
            //services.TryAddSingleton<IServiceProxy, ServiceProxy>();
            return services;
        }
    }
}
