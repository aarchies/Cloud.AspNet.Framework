using Abstaractions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBus;
using Glasssix.Contrib.EventBus;
using Glasssix.Contrib.EventBus.Abstractions;
using Glasssix.Contrib.EventBus.Rabbitmq;
using Glasssix.Contrib.EventBus.Rabbitmq.Option;
using Glasssix.Utils.ReflectionConductor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Linq;

namespace Glasssix.BuildingBlocks.EventCenter
{
    public static class EventBusModuleProvider
    {
        public static ITypeFinder TypeFinder = new TypeFinder();

        /// <summary>
        /// 启用EventBus
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <param name="registers">业务订阅handler类型</param>
        /// <returns></returns>
        public static IServiceCollection AddEventBusonRabbitmq(this IServiceCollection services, Action<EventBusOptions> action)
        {
            var option = new EventBusOptions();
            action.Invoke(option);

            #region RabbitMq

            services.AddSingleton<IDefaultRabbitMQConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQConnection>>();
                var retryCount = 10;
                var factory = new ConnectionFactory()
                {
                    DispatchConsumersAsync = true,
                };

                var clusterNode = option.Connection.Split(",");

                if (clusterNode.Count() == 1)
                    factory.HostName = option.Connection;

                if (!string.IsNullOrEmpty(option.Port))
                    factory.Port = Convert.ToInt32(option.Port);

                if (!string.IsNullOrEmpty(option.UserName))
                    factory.UserName = option.UserName;

                if (!string.IsNullOrEmpty(option.Password))
                    factory.Password = option.Password;

                if (!string.IsNullOrEmpty(option.RetryCount))
                    retryCount = int.Parse(option.RetryCount);

                return new DefaultRabbitMQConnection(factory, logger, clusterNode, retryCount);
            });

            #endregion RabbitMq

            #region EventBus

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var Exchange = option.Exchange;
                var subscriptionClientName = option.QueueName;
                var rabbitMQPersistentConnection = sp.GetRequiredService<IDefaultRabbitMQConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var retryCount = 5;

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, Exchange, subscriptionClientName, retryCount);
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            #endregion EventBus

            var typeAutos = TypeFinder.Find<IIntegrationEventHandler>(TypeFinder.GetAssemblies());
            foreach (var item in typeAutos)
            {
                services.AddTransient(item);
            }

            services.AddAutofac(delegate (ContainerBuilder x)
            {
                x.Populate(services);
                x.Build();
            });
            return services;
        }
    }
}