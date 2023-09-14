using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

///必须使用仓储层 内部实现强依赖仓储层封装  此处代码无效
namespace Volo.Mongo.Test
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            context.Services.AddControllers();
            context.Services.AddEndpointsApiExplorer();
            context.Services.AddSwaggerGen();
            context.Services.AddMongoDbContext<MyDbContext>();
            base.ConfigureServices(context);
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await context.ServiceProvider.GetRequiredService<IBookStoreDbSchemaMigrator>().MigrateAsync();
            await base.OnApplicationInitializationAsync(context);
        }

    }
}
