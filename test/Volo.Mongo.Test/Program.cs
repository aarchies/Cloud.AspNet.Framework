using Volo.Abp.DependencyInjection;
using Volo.Mongo.Test;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<ObjectAccessor<IApplicationBuilder>>();
await builder.AddApplicationAsync<MyModule>();
var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();
