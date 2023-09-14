namespace Glasssix.BuildingBlocks.Grpc.Client
{
    public interface IServiceProxy
    {
        /// <summary>
        /// 创建服务
        /// </summary>
        /// <typeparam name="TService">服务</typeparam>
        /// <returns></returns>
        TService CreateService<TService>(Uri uri) where TService : class;
        /// <summary>
        /// 创建代理
        /// </summary>
        /// <typeparam name="TService">服务</typeparam>
        /// <param name="host">主机地址</param>
        /// <returns></returns>
        TService CreateService<TService>(string host) where TService : class;
    }



  

    public class GrpcService
    {
        public Dictionary<string, string> Services { get; set; }
    }
}
