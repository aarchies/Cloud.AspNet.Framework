using Rpc.DemoService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//声明Rpc客户端连接地址
builder.Services.AddGrpcClient<RpcDemoService.RpcDemoServiceClient>(option =>
{
    option.Address = new Uri("https://localhost:7155");
    //option.Interceptors //拦截器
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
