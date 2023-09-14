using Glasssix.Contrib.Caller.Middleware;
using Glasssix.Contrib.Caller.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Glasssix.Contrib.Caller.Extensions
{
    public static class GlasssixCallerClientBuilderExtensions
    {
        public static IGlasssixCallerClientBuilder AddMiddleware<TMiddleware>(
                this IGlasssixCallerClientBuilder GlasssixCallerClientBuilder)
                where TMiddleware : class, ICallerMiddleware
        {
            GlasssixCallerClientBuilder.Services.TryAddSingleton<TMiddleware>();
            return GlasssixCallerClientBuilder.AddMiddleware(serviceProvider => serviceProvider.GetRequiredService<TMiddleware>());
        }

        public static IGlasssixCallerClientBuilder AddMiddleware(
                this IGlasssixCallerClientBuilder GlasssixCallerClientBuilder,
                Func<IServiceProvider, ICallerMiddleware> implementationFactory)
        {
            GlasssixCallerClientBuilder.Services.Configure<CallerMiddlewareFactoryOptions>(middlewareOptions =>
            {
                middlewareOptions.AddMiddleware(GlasssixCallerClientBuilder.Name, implementationFactory);
            });
            return GlasssixCallerClientBuilder;
        }

        // ReSharper disable once InconsistentNaming
        public static IGlasssixCallerClientBuilder UseI18n(this IGlasssixCallerClientBuilder GlasssixCallerClientBuilder)
            => GlasssixCallerClientBuilder.AddMiddleware(_ => new CultureMiddleware());
    }
}