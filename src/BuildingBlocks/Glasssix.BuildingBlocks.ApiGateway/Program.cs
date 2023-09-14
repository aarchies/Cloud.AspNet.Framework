using Glasssix.BuildingBlocks.ApiGateway.Handlers;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddOcelot(builder.Configuration)
        //服务发现
        .AddConsul()
        //缓存
        .AddCacheManager(x => { x.WithDictionaryHandle(); })
        //重试，熔断，降级.....
        .AddPolly()
        .AddDelegatingHandler<LoggerDelegatingHandler>(true); ;

var app = builder.Build();

app.UseRouting();
app.UseOcelot().Wait();
app.Run();