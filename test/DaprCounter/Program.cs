using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Rpc.DaprTestService;

var builder = WebApplication.CreateBuilder();
builder.Services.AddDaprClient(sp => { sp.UseHttpEndpoint("http://127.0.0.1:3500"); });

//.net7特有 [EnableRateLimiting("FixedWindow")]  特性注入即可
builder.Services.AddRateLimiter(p => p
    .AddFixedWindowLimiter(policyName: "FixedWindow", options =>
    {
        options.PermitLimit = 3;
        options.Window = TimeSpan.FromSeconds(10);
    }));



var app = builder.Build();
app.UseRateLimiter();
var dad = app.Services.GetRequiredService<Dapr.Client.DaprClient>();
var result = await dad.InvokeMethodGrpcAsync<HelloRequest, HelloReply>("grpcserver", "get", new HelloRequest() { Name = "method.Get" });
app.Run();