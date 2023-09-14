using Microsoft.AspNetCore.Builder;

namespace Glasssix.DotNet.Framework.Grpc
{
    internal interface IMapGrpcProxy
    {
        void Map(IApplicationBuilder builder);
    }

    internal class MapGrpcProxy<T1> : IMapGrpcProxy
        where T1 : class
    {
        public void Map(IApplicationBuilder builder)
        {
            builder.UseEndpoints(endpoint => { endpoint.MapGrpcService<T1>(); });
        }
    }
}