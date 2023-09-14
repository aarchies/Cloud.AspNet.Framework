using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Glasssix.BuildingBlocks.Grpc.Server.Internal
{
    public class LoggerInterceptor : Interceptor
    {
        private readonly ILogger<LoggerInterceptor> _logger;

        public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
        {
            _logger = logger;
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                Console.WriteLine("GrpcServer Start AsyncServerStreamingCall");
                return base.AsyncServerStreamingCall(request, context, continuation);
            }
            catch (RpcException e)
            {
                Console.WriteLine("异常了:" + e.Message);
                throw e;
            }
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream,
            IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                Console.WriteLine("GrpcServer Start DuplexStreamingServerHandler");
                return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
            }
            catch (RpcException e)
            {
                Console.WriteLine("异常了:" + e.Message);
                throw e;
            }
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                Console.WriteLine("GrpcServer Start ServerStreamingServerHandler");
                return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
            }
            catch (RpcException e)
            {
                Console.WriteLine("异常了:" + e.Message);
                throw e;
            }
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
                            UnaryServerMethod<TRequest, TResponse> continuation)
        {

            try
            {
                var response = await continuation(request, context);

                return response;
            }
            catch (RpcException e)
            {
                _logger.LogError($"UnaryServerHandler处理异常:{e.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnaryServerHandler处理异常:{ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), ex.Message);
            }
            //finally
            //{
            //    var httpContext = context.GetHttpContext();
            //    httpContext.Items.Add(ApmConfig.GrpcRequest, request);
            //    httpContext.Items.Add(ApmConfig.GrpcResponse, response);
            //}
            return default;
        }
    }
}