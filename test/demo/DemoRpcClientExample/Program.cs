using Rpc.DemoService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//����Rpc�ͻ������ӵ�ַ
builder.Services.AddGrpcClient<RpcDemoService.RpcDemoServiceClient>(option =>
{
    option.Address = new Uri("https://localhost:7155");
    //option.Interceptors //������
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
