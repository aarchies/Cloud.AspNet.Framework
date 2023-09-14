using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using Glasssix.Contrib.Caller.Infrastructure.Json;
using Glasssix.Contrib.Caller.Internal;
using Glasssix.Contrib.Caller.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glasssix.Contrib.Caller.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoRegistrationCaller(
            this IServiceCollection services,
            ServiceLifetime callerLifetime = ServiceLifetime.Scoped)
            => services.AddAutoRegistrationCaller(GlasssixIocApp.GetAssemblies(), callerLifetime);

        public static IServiceCollection AddAutoRegistrationCaller(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            ServiceLifetime callerLifetime = ServiceLifetime.Scoped)
        {
            services.AddCallerCore();
            services.AddAutomaticCaller(assemblies, callerLifetime);
            return services;
        }

        public static IServiceCollection AddAutoRegistrationCaller(
            this IServiceCollection services,
            params Assembly[] assemblies)
            => services.AddAutoRegistrationCaller(assemblies.AsEnumerable());

        public static IServiceCollection AddCaller(this IServiceCollection services, Action<CallerOptionsBuilder> configure)
                        => services.AddCaller(Microsoft.Extensions.Options.Options.DefaultName, configure);

        public static IServiceCollection AddCaller(this IServiceCollection services, string name, Action<CallerOptionsBuilder> configure)
        {
            services.AddCallerCore();

            var callerOption = new CallerOptionsBuilder(services, name);
            configure.Invoke(callerOption);

            return services;
        }

        public static IServiceCollection AddCaller(
            this IServiceCollection services,
            string name,
            Func<IServiceProvider, ICaller> implementationFactory)
        {
            services.Configure<CallerFactoryOptions>(callerOptions =>
            {
                if (callerOptions.Options.Any(relation => relation.Name!.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    throw new ArgumentException($"The caller name already exists, please change the name, the repeat name is [{name}]");

                callerOptions.Options.Add(new CallerRelationOptions(name, implementationFactory));
            });

            GlasssixIocApp.TrySetServiceCollection(services);
            return services;
        }

        private static void AddAutomaticCaller(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            ServiceLifetime callerLifetime)
        {
            var optionTypeList = assemblies.Where(x => !x.GetName().Name.Contains("MySql.EntityFrameworkCore"));
            var callerTypes = optionTypeList.SelectMany(x => x.GetTypes())
                .Where(type => typeof(CallerBase).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            callerTypes = callerTypes.Except(services.Select(d => d.ServiceType)).ToList();

            if (callerTypes.Count == 0)
                return;

            callerTypes.Arrangement().ForEach(type =>
            {
                var serviceDescriptor = new ServiceDescriptor(type, serviceProvider =>
                {
                    var constructorInfo = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Max();
                    //.MaxBy(constructor => constructor.GetParameters().Length)!;
                    List<object?> parameters = new();
                    foreach (var parameter in constructorInfo.GetParameters())
                    {
                        parameters.Add(serviceProvider.GetService(parameter.ParameterType));
                    }
                    var callerBase = (constructorInfo.Invoke(parameters.ToArray()) as CallerBase)!;

                    var name = callerBase.Name ?? type.FullName ?? type.Name;
                    callerBase.SetCallerOptions(new CallerOptionsBuilder(services, name), name);
                    if (callerBase.ServiceProvider == null) callerBase.SetServiceProvider(serviceProvider);

                    return callerBase;
                }, callerLifetime);
                services.TryAdd(serviceDescriptor);
            });

            var serviceProvider = services.BuildServiceProvider();
            callerTypes.ForEach(type =>
            {
                var callerBase = (CallerBase)serviceProvider.GetRequiredService(type);

                callerBase.UseCallerExtension();
            });
        }

        private static void AddCallerCore(this IServiceCollection services)
        {
            services.TryAddSingleton<ICallerFactory, DefaultCallerFactory>();
            services.TryAddSingleton<IRequestMessage>(_ => new JsonRequestMessage());
            services.TryAddSingleton<IResponseMessage>(_ => new JsonResponseMessage());
            services.TryAddSingleton(serviceProvider => serviceProvider.GetRequiredService<ICallerFactory>().Create());
            services.TryAddSingleton<ITypeConvertor, DefaultTypeConvertor>();
        }
    }
}