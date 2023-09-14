//using Glasssix.BuildingBlocks.Grpc.Client;
//using Microsoft.Extensions.DependencyInjection;

//namespace Glasssix.DotNet.Framework.Grpc
//{
//    public class ServiceProxy : IServiceProxy
//    {
//        private readonly IServiceCollection _services;
//        private readonly ClientLoggerInterceptor _interceptor;

//        public ServiceProxy(IServiceCollection builder, ClientLoggerInterceptor interceptor)
//        {
//            _services = builder;
//            _interceptor = interceptor;
//        }

//        public TService CreateService<TService>(Uri uri) where TService : class
//        {
//            var service = _services.BuildServiceProvider().GetService<TService>();
//            if (service != null)
//                return service;
//            _services.AddGrpcClient<TService>(options =>
//            {
//                options.Address = uri;
//                if (_interceptor != null)
//                    options.Interceptors.Add(_interceptor);
//            });
//            service = _services.BuildServiceProvider().GetRequiredService<TService>();
//            return service;
//        }

//        public TService CreateService<TService>(string uri) where TService : class
//        {
//            return CreateService<TService>(new Uri(uri));
//        }
//    }
//}