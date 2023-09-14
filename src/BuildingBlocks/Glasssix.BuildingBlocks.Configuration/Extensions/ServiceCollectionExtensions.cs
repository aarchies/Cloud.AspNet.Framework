using Glasssix.BuildingBlocks.Configuration.Internal;
using Glasssix.BuildingBlocks.Configuration.Options;
using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using Glasssix.Utils.Configuration;
using Glasssix.Utils.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glasssix.BuildingBlocks.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigure<TOptions>(this IServiceCollection services, string sectionName, string? name = null, bool isRoot = false) where TOptions : class
        {
            services.AddOptions();
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IGlasssixConfiguration>()?.Local ?? provider.GetService<IConfiguration>()!;
            if (configuration == null)
            {
                return services;
            }

            if (name == null)
            {
                name = Microsoft.Extensions.Options.Options.DefaultName;
            }

            IConfigurationSection section = configuration.GetSection(sectionName);
            if (!section.Exists())
            {
                return services;
            }

            string? name2 = name;
            IConfiguration config;
            if (!isRoot)
            {
                IConfiguration configuration2 = section;
                config = configuration2;
            }
            else
            {
                config = configuration;
            }

            services.Configure<TOptions>(name2, config);
            services.Configure<TOptions>(configuration);
            return services;
        }

        public static IServiceCollection AddGlasssixConfiguration(
            this IServiceCollection services,
            IEnumerable<Assembly>? assemblies = null)
            => services.AddGlasssixConfiguration(
                null,
                options =>
                {
                    if (assemblies != null) options.Assemblies = assemblies.ToArray();
                });

        public static IServiceCollection AddGlasssixConfiguration(
            this IServiceCollection services,
            Action<IGlasssixConfigurationBuilder>? configureDelegate,
            Action<ConfigurationOptions>? action = null)
        {
            services.InitializeAppConfiguration();

            var sourceConfiguration = services.BuildServiceProvider().GetService<IConfiguration>();

            var configurationBuilder = sourceConfiguration as IConfigurationBuilder ??
                (sourceConfiguration == null ? new ConfigurationBuilder() : new ConfigurationBuilder().AddConfiguration(sourceConfiguration));

            var GlasssixConfiguration =
                services.CreateGlasssixConfiguration(
                    configureDelegate,
                    configurationBuilder.Sources,
                    action);

            if (!GlasssixConfiguration.Providers.Any())
                return services;

            //configurationBuilder.Sources.Clear();
            configurationBuilder.AddConfiguration(GlasssixConfiguration);

            if (sourceConfiguration == null) services.AddSingleton<IConfiguration>(_ => configurationBuilder.Build());

            return services;
        }

        public static IGlasssixConfiguration GetGlasssixConfiguration(this IServiceCollection services)
            => services.BuildServiceProvider().GetRequiredService<IGlasssixConfiguration>();

        public static IConfiguration GetLocalConfiguration(this IServiceCollection services, string sectionName)
        {
            ServiceProvider provider = services.BuildServiceProvider();
            IConfiguration configuration = provider.GetService<IGlasssixConfiguration>()?.Local ?? provider.GetService<IConfiguration>();
            if (configuration == null)
            {
                throw new NotSupportedException();
            }

            return configuration.GetSection(sectionName);
        }

        public static IServiceCollection InitializeAppConfiguration(this IServiceCollection services)
        {
            if (services.Any(service => service.ImplementationType == typeof(InitializeAppConfigurationProvider)))
                return services;

            services.AddSingleton<InitializeAppConfigurationProvider>();

            GlasssixIocApp.TrySetServiceCollection(services);

            IConfiguration? migrateConfiguration = null;
            bool initialized = false;

            services.Configure<GlasssixAppConfigureOptions>(options =>
            {
                if (!initialized)
                {
                    var GlasssixConfiguration = services.BuildServiceProvider().GetService<IGlasssixConfiguration>();
                    if (GlasssixConfiguration != null) migrateConfiguration = GlasssixConfiguration.Local;
                    initialized = true;
                }
                var sourceConfiguration = services.BuildServiceProvider().GetService<IConfiguration>();

                if (string.IsNullOrWhiteSpace(options.AppId))
                    options.AppId = GetConfigurationValue(
                        options.GetVariable(nameof(options.AppId)),
                        sourceConfiguration,
                        migrateConfiguration);

                if (string.IsNullOrWhiteSpace(options.Environment))
                    options.Environment = GetConfigurationValue(
                        options.GetVariable(nameof(options.Environment)),
                        sourceConfiguration,
                        migrateConfiguration);

                if (string.IsNullOrWhiteSpace(options.Cluster))
                    options.Cluster = GetConfigurationValue(
                        options.GetVariable(nameof(options.Cluster)),
                        sourceConfiguration,
                        migrateConfiguration);

                foreach (var key in options.GetVariableKeys())
                {
                    options.TryAdd(key, GetConfigurationValue(
                        options.GetVariable(key),
                        sourceConfiguration,
                        migrateConfiguration));
                }
            });
            return services;
        }

        private static void ConfigureOption(
            this IServiceCollection services,
            IConfiguration configuration,
            List<string> sectionNames,
            Type optionType,
            string name)
        {
            IConfigurationSection? configurationSection = null;
            foreach (var sectionName in sectionNames)
            {
                if (configurationSection == null)
                    configurationSection = configuration.GetSection(sectionName);
                else
                    configurationSection = configurationSection.GetSection(sectionName);
            }
            if (!configurationSection.Exists())
                throw new ArgumentException($"Check if the mapping section is correct，section name is [{configurationSection!.Path}]");

            var configurationChangeTokenSource =
                Activator.CreateInstance(typeof(ConfigurationChangeTokenSource<>).MakeGenericType(optionType), name,
                    configurationSection)!;
            services.Add(new ServiceDescriptor(typeof(IOptionsChangeTokenSource<>).MakeGenericType(optionType),
                configurationChangeTokenSource));

            Action<BinderOptions> configureBinder = _ =>
            {
            };
            var configureOptions =
                Activator.CreateInstance(typeof(NamedConfigureFromConfigurationOptions<>).MakeGenericType(optionType),
                    name,
                    configurationSection, configureBinder)!;
            services.Add(new ServiceDescriptor(typeof(IConfigureOptions<>).MakeGenericType(optionType),
                configureOptions));
        }

        private static IConfigurationRoot CreateGlasssixConfiguration(
            this IServiceCollection services,
            Action<IGlasssixConfigurationBuilder>? configureDelegate,
            IEnumerable<IConfigurationSource> originalConfigurationSources,
            Action<ConfigurationOptions>? action)
        {
            if (services.Any(service => service.ImplementationType == typeof(GlasssixConfigurationProvider)))
                return new ConfigurationBuilder().Build();

            services.AddSingleton<GlasssixConfigurationProvider>();
            services.AddOptions();
            services.TryAddSingleton<IGlasssixConfigurationSourceProvider, DefaultGlasssixConfigurationSourceProvider>();
            services.TryAddSingleton<IGlasssixConfiguration, DefaultGlasssixConfiguration>();
            services.TryAddSingleton<IConfigurationApi, DefaultConfigurationApi>();
            var configurationOptions = new ConfigurationOptions();
            action?.Invoke(configurationOptions);

            var configurationSourceResult = services
                .BuildServiceProvider()
                .GetRequiredService<IGlasssixConfigurationSourceProvider>()
                .GetMigrated(
                    originalConfigurationSources,
                    configurationOptions.ExcludeConfigurationSourceTypes,
                    configurationOptions.ExcludeConfigurationProviderTypes);

            GlasssixConfigurationBuilder GlasssixConfigurationBuilder = new GlasssixConfigurationBuilder(services,
                new ConfigurationBuilder().AddRange(configurationSourceResult.MigrateConfigurationSources));
            configureDelegate?.Invoke(GlasssixConfigurationBuilder);

            GlasssixConfigurationBuilder builder = new(services, new ConfigurationBuilder());
            builder.AddRelations(GlasssixConfigurationBuilder.Relations.ToArray());
            GlasssixConfigurationBuilder.Repositories.ForEach(repository => builder.AddRepository(repository));
            var localConfigurationRepository = new LocalGlasssixConfigurationRepository(
                GlasssixConfigurationBuilder.Configuration);
            builder.AddRepository(localConfigurationRepository);

            var source = new GlasssixConfigurationSource(builder);
            var configuration = builder
                .Add(source)
                .AddRange(configurationSourceResult.ConfigurationSources)
                .Build();

            builder.AutoMapping(configurationOptions.Assemblies);

            builder.Relations.ForEach(relation =>
            {
                List<string> sectionNames = new()
                {
                relation.SectionType.ToString(),
                };
                if (!string.IsNullOrEmpty(relation.ParentSection))
                    sectionNames.Add(relation.ParentSection);

                if (relation.Section != "")
                {
                    sectionNames.AddRange(relation.Section!.Split(ConfigurationPath.KeyDelimiter));
                }

                services.ConfigureOption(configuration, sectionNames, relation.ObjectType, relation.Name);
            });

            return configuration;
        }

        private static string GetConfigurationValue(VariableInfo? variableInfo,
                    IConfiguration? configuration,
            IConfiguration? migrateConfiguration)
        {
            var value = string.Empty;
            if (variableInfo == null) return value;

            if (configuration != null)
            {
                value = configuration[variableInfo.Variable];
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }

            if (migrateConfiguration != null)
                value = migrateConfiguration[variableInfo.Variable];
            if (string.IsNullOrWhiteSpace(value))
                value = variableInfo.DefaultValue;
            return value;
        }

        private sealed class GlasssixConfigurationProvider
        {
        }

        private sealed class InitializeAppConfigurationProvider
        {
        }
    }
}