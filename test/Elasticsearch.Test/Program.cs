using Apm.Test;
using Glasssix.Contrib.Data.Elasticsearch;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Get;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Set;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddElasticsearch(s =>
{
    s.UseNodes("http://elastic:e4R5bqtmaO5mq6044G9R9c2f@10.168.1.47:32133");
});

var app = builder.Build();
var clienta = app.Services.GetRequiredService<IGlasssixElasticClient>();

var model = new Company();
model.Name = "Test";
model.Description = "测试";

//判断是否存在
var resulte = await clienta.IndexExistAsync("gzf_ytst");

//创建索引 并映射对应模型
//var result = await clienta.CreateIndexAsync("gzf_ytst", o => o.Map(x => x.AutoMap<Company>()));

//添加数据
var aresultset = await clienta.SetDocumentAsync(new SetDocumentRequest<Company>("gzf_ytst", model));

//查询数据
await clienta.GetAsync<Company>(new GetDocumentRequest("", string.Empty));
app.Run();
