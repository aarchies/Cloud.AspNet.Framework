using Abstaractions;
using DotXxlJob.Core;
using Glasssix.Contrib.Scheduler.xxljob;
using Glasssix.Contrib.Scheduler.xxljob.Abstaractions;
using Glasssix.Contrib.Scheduler.xxljob.Extensions;
using Glasssix.Contrib.Scheduler.xxljob.Extensions.Dto;
using Glasssix.Utils.ReflectionConductor;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.BuildingBlocks.Scheduler
{
    public static class SchedulerClientProvider
    {
        public static ITypeFinder TypeFinder = new TypeFinder();

        /// <summary>
        /// 启用xxlJob任务调度
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="types">手动注册handler类</param>
        /// <returns></returns>
        public static IServiceCollection AddxxlJobClient(this IServiceCollection services, IConfiguration configuration, RegisterHandlerTypes types = null)
        {
            services.AddXxlJobExecutor(configuration);
            //手动注册
            if (types != null)
            {
                foreach (var item in types.Types)
                    services.AddSingleton(typeof(IJobHandler), item);
                Console.WriteLine($"发现 {types.Types.Count} 个Job任务：\r\n[ {string.Join(",", types.Types.Select(i => i.Name.ToString()))} ]\r");
            }
            else
            {
                //自动注册
                var typeAutos = TypeFinder.Find<AbstractJobHandler>(TypeFinder.GetAssemblies());
                foreach (var item in typeAutos)
                    services.AddSingleton(typeof(IJobHandler), item);

                Console.WriteLine($"发现 {typeAutos.Count} 个Job任务：\r\n[ {string.Join(",", typeAutos.Select(i => i.Name.ToString()))} ]\r");
            }
            services.AddAutoRegistry();

            return services.AddSingleton<IBaseServiceFactory, BaseServiceFactory>(sp =>
            {
                return new BaseServiceFactory(configuration.GetSection("xxlJob").Get<BuilderOption>(), sp.GetRequiredService<ILoggerFactory>());
            });
        }

        /// <summary>
        /// xxlJob中间件注入
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseXxlJobExecutor(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<XxlJobExecutorMiddleware>();
        }

        public class RegisterHandlerTypes
        {
            public List<Type> Types { get; set; }
        }
    }
}