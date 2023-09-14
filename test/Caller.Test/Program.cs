using Glasssix.Contrib.Caller.DaprClient.Test;
using Glasssix.Contrib.Caller.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.AddGlasssixFramework();

builder.Services.AddAutoRegistrationCaller();

//builder.Services.AddDaprClient();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/get", async ([FromServices] CustomDaprCaller callerFactory) =>
{
    var result = await callerFactory.GetRpcAsync();
    return result;
});
//var dad = app.Services.GetRequiredService<Dapr.Client.DaprClient>();
//var result = await dad.InvokeMethodGrpcAsync<HelloRequest, HelloReply>("grpcserver", "get", new HelloRequest() { Name = "hello" });

//app.UseGlasssixFramework(app.Lifetime);

app.Run();