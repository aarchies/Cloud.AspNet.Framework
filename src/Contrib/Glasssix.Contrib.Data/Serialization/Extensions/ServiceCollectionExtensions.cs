using Glasssix.Contrib.Data.Serialization;
using Glasssix.Contrib.Data.Serialization.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static void TryAddSerializationCore(this IServiceCollection services)
        {
            services.TryAddSingleton<IDeserializerFactory, DefaultDeserializerFactory>();
            services.TryAddSingleton<ISerializerFactory, DefaultSerializerFactory>();
            services.TryAddSingleton(serviceProvider => serviceProvider.GetRequiredService<ISerializerFactory>().Create());
            services.TryAddSingleton(serviceProvider => serviceProvider.GetRequiredService<IDeserializerFactory>().Create());
        }
    }
}