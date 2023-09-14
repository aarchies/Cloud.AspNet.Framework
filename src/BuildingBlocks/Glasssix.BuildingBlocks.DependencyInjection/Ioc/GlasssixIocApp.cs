using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Glasssix.BuildingBlocks.DependencyInjection.Ioc
{
    /// <summary>
    /// 基础默认容器
    /// </summary>
    public static class GlasssixIocApp
    {
        private static IHostEnvironment? _environment;

        private static IServiceProvider? _rootServiceProvider;

        public static ILifetimeScope lifetimeScope
        {
            get
            {
                return GetIServiceProvider().GetService<ILifetimeScope>()!;
            }
            private set => lifetimeScope = value;
        }

        public static IServiceProvider RootServiceProvider
        {
            get
            {
                if (_rootServiceProvider == null) Build();
                return _rootServiceProvider!;
            }
            private set => _rootServiceProvider = value;
        }

        private static IEnumerable<Assembly>? Assemblies { get; set; }
        private static JsonSerializerOptions? JsonSerializerOptions { get; set; }
        private static IServiceCollection Services { get; set; } = new ServiceCollection();

        #region Env

        public static IHostEnvironment TryGetEnvironment()
        {
            return _environment!;
        }

        public static void TrySetEnvironment(IHostEnvironment environment) => _environment = environment;

        #endregion Env

        #region Build

        public static void Build() => Build(Services.BuildServiceProvider());

        public static void Build(IServiceProvider serviceProvider) => RootServiceProvider = serviceProvider;

        #endregion Build

        #region GetService

        public static TService GetRequiredService<TService>() where TService : notnull
            => GetRequiredService<TService>(RootServiceProvider);

        public static TService GetRequiredService<TService>(IServiceProvider serviceProvider) where TService : notnull
        {
            return GetRequiredServiceOnLifetime<TService>() ?? serviceProvider.GetRequiredService<TService>();
        }

        public static TService? GetRequiredServiceOnLifetime<TService>() where TService : notnull
        {
            if (lifetimeScope != null)
                return lifetimeScope!.Resolve<TService>();

            return default(TService);
        }

        public static TService? GetService<TService>()
            => GetService<TService>(RootServiceProvider);

        public static TService? GetService<TService>(IServiceProvider serviceProvider)
            => serviceProvider.GetService<TService>();

        public static IServiceCollection GetServices() => Services;

        #endregion GetService

        #region SetService

        public static void SetServiceCollection(IServiceCollection services)
        {
            Services = services;
            _rootServiceProvider = null;
        }

        public static void TrySetServiceCollection(IServiceCollection services)
        {
            if (Services.Count == 0) SetServiceCollection(services);
        }

        #endregion SetService

        #region SetAssemblies

        /// <summary>
        /// Set the global Assembly collection
        /// </summary>
        /// <param name="assemblies"></param>
        public static void SetAssemblies(params Assembly[] assemblies)
            => Assemblies = assemblies;

        /// <summary>
        /// Set the global Assembly collection
        /// </summary>
        /// <param name="assemblies"></param>
        public static void SetAssemblies(IEnumerable<Assembly> assemblies)
            => Assemblies = assemblies;

        /// <summary>
        /// Set the global Assembly collection (only if Assembly is not assigned a value)
        /// </summary>
        /// <param name="assemblies"></param>
        public static void TrySetAssemblies(params Assembly[] assemblies)
        {
            //ArgumentNullException.ThrowIfNull(assemblies);

            Assemblies ??= assemblies;
        }

        /// <summary>
        /// Set the global Assembly collection (only if Assembly is not assigned a value)
        /// </summary>
        /// <param name="assemblies"></param>
        public static void TrySetAssemblies(IEnumerable<Assembly> assemblies)
        {
            //ArgumentNullException.ThrowIfNull(assemblies);

            Assemblies ??= assemblies.ToArray();
        }

        #endregion SetAssemblies

        #region GetAssemblies

        public static IEnumerable<Assembly> GetAssemblies() => Assemblies ?? AppDomain.CurrentDomain.GetAssemblies();

        #endregion GetAssemblies

        #region SetJsonSerializerOptions

        public static JsonSerializerOptions? GetJsonSerializerOptions() => JsonSerializerOptions;

        public static void SetJsonSerializerOptions(JsonSerializerOptions jsonSerializerOptions)
            => JsonSerializerOptions = jsonSerializerOptions;

        public static void TrySetJsonSerializerOptions(JsonSerializerOptions jsonSerializerOptions)
        {
            if (JsonSerializerOptions == null) SetJsonSerializerOptions(jsonSerializerOptions);
        }

        #endregion SetJsonSerializerOptions

        #region SetIServiceProvider

        public static void SetIServiceProvider(IServiceProvider services)
        {
            RootServiceProvider = services;
        }

        #endregion SetIServiceProvider

        #region GetIServiceProvider

        public static IServiceProvider GetIServiceProvider()
        {
            return RootServiceProvider;
        }

        #endregion GetIServiceProvider
    }
}