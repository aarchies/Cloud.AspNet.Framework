using Glasssix.Contrib.Message.Emqx;
using Glasssix.Contrib.Message.Emqx.Client;
using Glasssix.Contrib.Message.Emqx.MultipleImplementation;
using Glasssix.Contrib.Message.Emqx.MultipleImplementation.Attribuites;
using Glasssix.Contrib.Message.Emqx.MultipleImplementation.Attribuites.Option;
using Glasssix.Contrib.Message.Emqx.MultipleImplementation.Workers;
using Glasssix.Contrib.Message.Emqx.Option;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;

namespace Glasssix.BuildingBlocks.MessageCenter
{
    public static class MqttClientProvider
    {
        /// <summary>
        /// 启用emqx
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddMqttxClient(this IServiceCollection services, Action<MqttOptions> action)
        {
            var option = new MqttOptions();

            action.Invoke(option);

            var factory = LoggerFactory.Create(builder => builder.AddConsole());

            var instance = new MqttClient(factory, option);

            services.AddSingleton<IMqttClient>(instance);

            return services;
        }

        /// <summary>
        /// 启用Emqx 特性注入方式进行使用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <param name="mapAssemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddMqttClient(this IServiceCollection services, Action<MqttOptions> action, params Assembly[] mapAssemblies)
        {
            services.Configure(action);

            services.AddSingleton<IMessageQueueClient, EMQClient>();
            var handlers = mapAssemblies
               .SelectMany(x => x.GetTypes())
               .Select(x => (attr: x.GetCustomAttribute<MessageAttribute>(), handler: x))
               .Where(x => x.attr != null)
               .ToDictionary(x => (x.attr!.RouteKey, x.attr!.Group), y => y.handler);


            var map = new RouterHandlerTypeMap();

            foreach (var item in handlers)
            {
                services.AddScoped(item.Value);

                map.Add(new(item.Key.RouteKey, item.Key.Group), item.Value);
            }

            services.AddSingleton(map);

            services.AddHostedService<EMQWorker>();

            return services;
        }
    }
}