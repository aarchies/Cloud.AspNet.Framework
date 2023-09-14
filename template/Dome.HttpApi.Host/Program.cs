using Demo.EntityFrameWorkCore;
using Dome.HttpApi.Host;
using Glasssix.DotNet.Framework;
using Glasssix.DotNet.Framework.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


#region swagger
var identityUrl = builder.Configuration["IdentityClient:IdentityUrl"];
var clientId = builder.Configuration["IdentityClient:ClientId"];
var api = builder.Configuration["IdentityClient:Api"];
var scope = builder.Configuration["IdentityClient:Scope"];
var secret = builder.Configuration["IdentityClient:Secret"];
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TestApi - Test HTTP API",
        Version = "v1",
        Description = "The Test Service HTTP API"
    });
    option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Password = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(identityUrl + "/connect/authorize"),
                TokenUrl = new Uri(identityUrl + "/connect/token"),
                Scopes = new Dictionary<string, string> { { scope, "Test API - full access" } },
            }
        }
    });

    //�쳣�������
    option.OperationFilter<AuthorizeCheckOperationFilter>();
    //�ӿڱ�ע��Ϣ
    var file = Path.Combine(AppContext.BaseDirectory, $"{builder.Environment.ApplicationName}.xml");  // xml�ĵ�����·��
    var path = Path.Combine(AppContext.BaseDirectory, file); // xml�ĵ�����·��
    option.IncludeXmlComments(path, true); // true : ��ʾ��������ע��
    option.OrderActionsBy(o => o.RelativePath);
    option.EnableAnnotations();
});

#endregion


builder.AddGlasssixFramework()
    .AddIdentityClient(identityUrl, api, scope, secret)
    .AddDbContext<DemoDbContext>(builder.Configuration["ConnectionString"])
    .AddOrmDapper(builder.Configuration["ConnectionString"], false)
    .AddCache(sp => builder.Configuration.GetSection("RedisConfig").Bind(sp))
    .AddEventBus(sp => builder.Configuration.GetSection("rabbitmq").Bind(sp));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.OAuthUsePkce();
        setup.OAuthConfigObject = new Swashbuckle.AspNetCore.SwaggerUI.OAuthConfigObject
        {
            AppName = "ʾ������ Swagger UI",
            ClientId = clientId,
            ClientSecret = secret,
            Scopes = new string[] { scope },
        };
    });
}

app.UseGlasssixFramework(app.Lifetime);

app.Run();

