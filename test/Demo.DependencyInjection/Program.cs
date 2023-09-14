using Glasssix.Contrib.Caller.HttprClient.Test;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCaller("service1", options =>
//{
//    options.UseHttpClient(client => client.BaseAddress = "https://localhost:7003");
//});
//builder.Services.AddAutoRegistrationCaller();

var app = builder.Build();

//app.MapGet("/get1", async ([FromServices] ICallerFactory callerFactory, string name)
//    =>
//{
//    var userCaller = callerFactory.Create("service1");
//    var result = await userCaller.GetAsync<string>($"/Test/UserHelloGet", new { Name = name });

//    return result;
//}); // 获取到的是UserCaller

app.MapGet("/Test/User/Hello", ([FromServices] CustomCaller caller, string name) => caller.HelloAsync(name));

app.Run();