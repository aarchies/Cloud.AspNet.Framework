using Glasssix.BuildingBlocks.ApiGateway.Handlers;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddOcelot(builder.Configuration)
        //������
        .AddConsul()
        //����
        .AddCacheManager(x => { x.WithDictionaryHandle(); })
        //���ԣ��۶ϣ�����.....
        .AddPolly()
        .AddDelegatingHandler<LoggerDelegatingHandler>(true); ;

var app = builder.Build();

app.UseRouting();
app.UseOcelot().Wait();
app.Run();