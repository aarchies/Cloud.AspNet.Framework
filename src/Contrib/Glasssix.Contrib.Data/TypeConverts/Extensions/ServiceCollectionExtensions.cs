using Glasssix.Contrib.Data.Serialization.Interfaces;
using Glasssix.Contrib.Data.TypeConverts;
using Glasssix.Contrib.Data.TypeConverts.Interfaces;
using Glasssix.Contrib.Data.TypeConverts.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTypeConvert(this IServiceCollection services)
        {
            services
                .AddTypeConvertCore()
                .Configure<TypeConvertFactoryOptions>(option =>
            {
                option.TryMapping(serviceProvider => new DefaultTypeConvertProvider(serviceProvider.GetRequiredService<IDeserializerFactory>().Create()));
            });
            return services;
        }

        public static IServiceCollection AddTypeConvert(this IServiceCollection services, string name)
        {
            services
                .AddTypeConvertCore()
                .Configure<TypeConvertFactoryOptions>(option =>
            {
                option.TryMapping(name,
                    serviceProvider => new DefaultTypeConvertProvider(serviceProvider.GetRequiredService<IDeserializerFactory>().Create(name)));
            });
            return services;
        }

        private static IServiceCollection AddTypeConvertCore(this IServiceCollection services)
        {
            services.TryAddSingleton<ITypeConvertFactory, DefaultTypeConvertFactory>();
            services.TryAddSingleton(serviceProvider => serviceProvider.GetRequiredService<ITypeConvertFactory>().Create());
            return services;
        }
    }
}