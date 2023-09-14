using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Rpc.DaprTestService;

namespace Dapr.Api.Test
{
    public class DaprTestRpcServer : Dapr.AppCallback.Autogen.Grpc.v1.AppCallback.AppCallbackBase
    {
        private readonly ILogger<DaprTestRpcServer> _logger;

        public DaprTestRpcServer(ILogger<DaprTestRpcServer> logger)
        {
            _logger = logger;
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var result = new ListTopicSubscriptionsResponse();
            result.Subscriptions.Add(new TopicSubscription
            {
                PubsubName = "pubsub",
                Topic = "get"
            });
            return Task.FromResult(result);
        }

        /// <summary>
        /// Dapr下rpc固定通过此接口进行通信
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            var response = new InvokeResponse();

            switch (request.Method)
            {
                case "get":
                    _logger.LogInformation($"Hello Rpc DaprTest Get!");
                    var input = request.Data.Unpack<HelloRequest>();
                    var dataReply = new HelloReply { Message = $"Success!{input.Name}" };
                    response.Data = Any.Pack(dataReply);

                    break;
            }

            return await Task.FromResult(response);
        }
    }
}