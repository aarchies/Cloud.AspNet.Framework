using Volo.Abp.Guids;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddMqttClient(sp =>
//{
//    sp.Ip = "10.168.1.47";
//    sp.Port = 30014;
//    sp.Username = "system";
//    sp.Password = "1234qwer";
//    sp.ClientId = "testemqxshared111" + Guid.NewGuid().ToString()[0..5];
//},
//Assembly.GetExecutingAssembly());
builder.Services.Configure<AbpSequentialGuidGeneratorOptions>(s =>
{
    s.DefaultSequentialGuidType = SequentialGuidType.SequentialAtEnd;
});
builder.Services.AddTransient<IGuidGenerator, SequentialGuidGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
var idgenerator = app.Services.GetRequiredService<IGuidGenerator>();
var id = idgenerator.Create();
app.Run();

