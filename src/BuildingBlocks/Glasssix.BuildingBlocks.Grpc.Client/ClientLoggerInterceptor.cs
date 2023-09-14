using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Glasssix.BuildingBlocks.Grpc.Client
{
    public class ClientLoggerInterceptor : Interceptor
    {
        private readonly ILogger<ClientLoggerInterceptor> _logger;

        public ClientLoggerInterceptor(ILogger<ClientLoggerInterceptor> logger)
        {
            _logger = logger;
        }
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("========================AsyncUnaryCall");
            try
            {
                //var options = new CallOptions();
                //context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
                //var response = continuation(request, context);
                //return response;
                return base.AsyncUnaryCall(request, context, continuation);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
            BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("========================BlockingUnaryCall");
            return base.BlockingUnaryCall(request, context, continuation);
        }


        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            Console.WriteLine("========================UnaryServerHandler");
            return base.UnaryServerHandler(request, context, continuation);
        }

        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
            AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            Console.WriteLine("========================AsyncClientStreamingCall");
            return base.AsyncClientStreamingCall(context, continuation);
        }
    }
}