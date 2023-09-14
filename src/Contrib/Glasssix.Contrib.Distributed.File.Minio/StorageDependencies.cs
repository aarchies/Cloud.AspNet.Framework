using Glasssix.Contrib.File.Storage.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Glasssix.Contrib.File.Storage
{
    public static class StorageDependencies
    {
        /// <summary>
        /// 启用Minio
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddMinio(this IServiceCollection services, Action<MinioOptions> action)
        {
            var option = new MinioOptions();
            action(option);
            return services.AddSingleton<IMinioClient>(sp => new MinioClients(option));
        }
    }
}