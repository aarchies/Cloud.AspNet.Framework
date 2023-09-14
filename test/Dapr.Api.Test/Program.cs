using Dapr.Api.Test;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DaprTestRpcServer>();
});

app.Run();