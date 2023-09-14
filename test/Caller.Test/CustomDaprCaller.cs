using Dapr.Client;
using Glasssix.BuildingBlocks.DependencyInjection.Ioc;
using Glasssix.Contrib.Caller.Extensions;
using Microsoft.Extensions.Options;
using Rpc.DaprTestService;

namespace Glasssix.Contrib.Caller.DaprClient.Test
{
    public class CustomDaprCaller : DaprCallerBase
    {
        //服务地址配置
        public override Action<DaprClientBuilder> Configure { get; set; } = new Action<DaprClientBuilder>(sp =>
        {
            var config = GlasssixIocApp.GetRequiredService<IOptions<AppConfig>>();
            sp.UseHttpEndpoint(config.Value.HttpEndpoint);
            //sp.UseGrpcEndpoint(config.Value.GrpcEndpoint);
        });

        protected override string AppId { get; set; }

        /// <summary>
        /// 获取Get请求
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetAsync() => await Caller.GetAsync<string>($"/get");

        /// <summary>
        /// 通过Grpc获取Get请求
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRpcAsync()
        {
            await GetAsync();
            var result = await this.Caller.SendGrpcAsync<HelloRequest, HelloReply>("get", new HelloRequest() { Name = "method.Get" });
            return result.Message;
        }

        //重写注入事件增加中间件
        protected override GlasssixDaprClientBuilder UseDapr()
        {
            this.AppId = GlasssixIocApp.GetRequiredService<IOptions<AppConfig>>().Value.AppId;
            var daprClientBuilder = base.UseDapr();
            daprClientBuilder.AddMiddleware<LogCallerMiddleware>();
            return daprClientBuilder;
        }
    }

    public class LogCallerMiddleware : ICallerMiddleware
    {
        public Task HandleAsync(GlasssixHttpContext GlasssixHttpContext, CallerHandlerDelegate next, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("请求Get接口");

            return next();
        }
    }
}