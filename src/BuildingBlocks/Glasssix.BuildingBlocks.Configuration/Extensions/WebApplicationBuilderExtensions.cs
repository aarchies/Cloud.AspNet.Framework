//using Glasssix.BuildingBlocks.Configuration.Options;
//using Glasssix.Utils.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Reflection;

//namespace Glasssix.BuildingBlocks.Configuration.Extensions
//{
//    public static class WebApplicationBuilderExtensions
//    {
//        public static IServiceCollection AddGlasssixConfiguration(
//            this IServiceCollection builder,
//            IEnumerable<Assembly>? assemblies = null)
//        {
//            builder.AddGlasssixConfiguration(assemblies);
//            return builder;
//        }

//        public static IServiceCollection AddGlasssixConfiguration(
//            this IServiceCollection builder,
//            Action<IGlasssixConfigurationBuilder>? configureDelegate,
//            Action<ConfigurationOptions>? action = null)
//        {
//            builder.AddGlasssixConfiguration(configureDelegate, action);
//            return builder;
//        }

//        public static IGlasssixConfiguration GetGlasssixConfiguration(this IServiceCollection builder)
//            => builder.BuildServiceProvider().GetRequiredService<IGlasssixConfiguration>();

//        public static IServiceCollection InitializeAppConfiguration(this IServiceCollection builder)
//        {
//            builder.InitializeAppConfiguration();
//            return builder;
//        }
//    }
//}