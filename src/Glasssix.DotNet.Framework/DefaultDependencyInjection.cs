using Abstaractions;
using Autofac.Extensions.DependencyInjection;
using FireflySoft.RateLimit.AspNetCore;
using Glasssix.BuildingBlocks.Configuration.Extensions;
using Glasssix.BuildingBlocks.DependencyInjection;
using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using Glasssix.BuildingBlocks.Logging.Logger;
using Glasssix.BuildingBlocks.Scheduler;
using Glasssix.Contrib.Caching.ClientFactory.Multilevel;
using Glasssix.Contrib.EventBus.Abstractions;
using Glasssix.Contrib.ServiceDiscovery;
using Glasssix.DotNet.Framework.Const;
using Glasssix.DotNet.Framework.Extensions;
using Glasssix.DotNet.Framework.Grpc;
using Glasssix.DotNet.Framework.Runtime;
using Glasssix.Utils.ReflectionConductor;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Glasssix.DotNet.Framework
{

    /// <summary>
    /// 框架依赖注入类
    /// </summary>
    public static class DefaultDependencyInjection
    {
        /// <summary>
        /// 框架基础依赖注入
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IGlasssixBuilder AddGlasssixFramework(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions();
            builder.WebHost.UseSerilogDefault();
            var builders = new GlasssixBuilder(builder);
            builder.Services.AddMvc();
            builder.Services.AddEndpointsApiExplorer()
              .AddSingleton<IGlasssixBuilder, GlasssixBuilder>(sp => builders)
              .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
              .AddSingleton<ITypeFinder, TypeFinder>()
              .AddHttpClient()
              .AddServiceRegistrator()
              .TryAddSerializationCore();
            builder.Services.AddGrpc(options => { options.EnableDetailedErrors = true; });
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true).AddNewtonsoftJson();
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).UseConsoleLifetime();
            GlasssixAspNetCoreModules.HealthChecksBuilder = builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
            builder.Services.AddGlasssixConfiguration();
            GlasssixIocApp.TrySetServiceCollection(builder.Services);
            return builders;
        }

        /// <summary>
        /// 管道注入
        /// </summary>
        /// <param name="app"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static WebApplication UseGlasssixFramework(this WebApplication app, IHostApplicationLifetime lifetime)
        {
            app.Services.GetAutofacRoot();
            GlasssixIocApp.SetIServiceProvider(app.Services);
            app.UseRouting();
            if (AppsettingConst.IsAuthorization)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            app.UseGrpcServer();

            app.MapDefaultControllerRoute();
            app.MapControllers();
            app.MapHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            GlasssixIocApp.TrySetEnvironment(app.Environment);

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                Log.Debug("Glasssix AspNet Framework Module loading completed");
                if (AppsettingConst.IsConsul)
                    app.UseConsul(lifetime, GlasssixAspNetCoreModules.ConsulOptions!);
                if (AppsettingConst.IsScheduler)
                    app.UseXxlJobExecutor();
                if (AppsettingConst.IsEventBus)
                    app.Services.GetRequiredService<IEventBus>().HealthyCheck();
                if (AppsettingConst.IsCache)
                    app.Services.GetRequiredService<IMultilevelCacheClient>();
                if (AppsettingConst.IsRateLimit)
                {
#if NET7_0

                    app.UseRateLimiter();
#endif
                    app.UseRateLimit();
                }
            });
            return app;
        }
    }
}