using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Glasssix.Contrib.ServiceDiscovery
{
    /// <summary>
    /// consul支持
    /// </summary>
    public static class ServiceDiscoveryExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection service, Action<ConsulOptions> action)
        {
            var option = new ConsulOptions();
            action.Invoke(option);
            //ConsulOptions option = configuration.GetSection("Consul").Get<ConsulOptions>();

            if (option.Enabled)
            {
                service.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
                {
                    //Consul主机地址
                    if (!string.IsNullOrEmpty(option.Host))
                    {
                        cfg.Address = new Uri(option.Host);
                    }
                }));
            }
            return service;
        }

        public static void UseConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, Action<ConsulOptions> action)
        {
            var option = new ConsulOptions();
            action.Invoke(option);
            using (var scope = app.ApplicationServices.CreateScope())
            {
                //var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                //ConsulOptions option = configuration.GetSection("Consul").Get<ConsulOptions>();

                if (option.Enabled)
                {
                    string serviceId = Guid.NewGuid().ToString("N");
                    string consulServiceID = $"{option.App!.Name}:{serviceId}";

                    var client = scope.ServiceProvider.GetService<IConsulClient>();
                    string healthCheckPath =
                        $"{option.App.Scheme}://{option.App.Host}:{option.App.Port.ToString()}/{option.HealthCheckPath}";

                    //健康检查
                    var httpCheck = new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(2), //服务启动多久后注册
                        Interval = TimeSpan.FromSeconds(10), //间隔固定的时间访问一次
                        HTTP = healthCheckPath, //健康检查地址
                        Timeout = TimeSpan.FromSeconds(5)
                    };

                    var consulServiceRistration = new AgentServiceRegistration
                    {
                        ID = consulServiceID,
                        Name = option.App.Name,
                        Address = option.App.Host,
                        Port = option.App.Port,
                        Tags = option.App.Tags,
                        Checks = new[] { httpCheck }
                    };

                    client!.Agent.ServiceRegister(consulServiceRistration).Wait();

                    lifetime.ApplicationStopping.Register(() =>
                    {
                        client.Agent.ServiceDeregister(consulServiceRistration.ID).Wait();
                    });
                }
            }
        }
    }
}