using Abstaractions;
using Glasssix.BuildingBlocks.DependencyInjection.Abstaractions;
using Glasssix.Utils.ReflectionConductor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.BuildingBlocks.DependencyInjection
{
    /// <summary>
    /// 依赖引导器
    /// </summary>
    public static class DependencyRegistrator
    {
        public static ITypeFinder TypeFinder = new TypeFinder();
        public static List<Type> Types = new List<Type>();

        /// <summary>
        /// 自动注册
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceRegistrator(this IServiceCollection services)
        {
            Console.WriteLine("AutoRegister Application Service ...");
            services.RegisterScopeDependency();
            services.RegisterSingleDependency();
            services.RegisterTransientDependency();
            return services;
        }

        private static void RegisterScopeDependency(this IServiceCollection services)
        {
            var types = TypeFinder.Find<IScopeDependency>(TypeFinder.GetAssemblies().ToList()).ToList();
            if (!types.Any())
                return;
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces.Where(x => x != typeof(IScopeDependency)))
                {
                    Console.WriteLine($"IScopeDependency:\t{type.Name}\t---->\t{@interface.Name}");
                    if (type.Name.Contains("Repository")) Types.Add(type);
                    services.AddScoped(@interface, type);
                }
            }
        }

        private static void RegisterSingleDependency(this IServiceCollection services)
        {
            var types = TypeFinder.Find<ISingletonDependency>(TypeFinder.GetAssemblies().ToList()).ToList();

            if (!types.Any()) return;
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces.Where(x => x != typeof(ISingletonDependency)))
                {
                    Console.WriteLine($"ISingletonDependency:\t{type.Name}\t---->\t{@interface.Name}");
                    if (type.Name.Contains("Repository")) Types.Add(type);

                    services.AddSingleton(@interface, type);
                }
            }
        }

        private static void RegisterTransientDependency(this IServiceCollection services)
        {
            var types = TypeFinder.Find<ITransientDependency>(TypeFinder.GetAssemblies().ToList()).ToList();
            if (!types.Any())
                return;
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces.Where(x => x != typeof(IScopeDependency)))
                {
                    Console.WriteLine($"ITransientDependency:\t{type.Name}\t---->\t{@interface.Name}");
                    if (type.Name.Contains("Repository")) Types.Add(type);
                    services.AddTransient(@interface, type);
                }
            }
        }
    }
}