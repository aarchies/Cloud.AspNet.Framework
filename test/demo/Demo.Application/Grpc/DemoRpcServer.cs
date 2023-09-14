using Glasssix.BuildingBlocks.Grpc.Server;
using Google.Protobuf;
using Grpc.Core;
using Helper.Grpc.Core;
using Microsoft.Extensions.Logging;
using Rpc.DemoService;
using System.Text.Json;

namespace Demo.Application.Grpc
{
    [GrpcServer]
    public class DemoRpcServer : RpcDemoService.RpcDemoServiceBase
    {
        private readonly ILogger<DemoRpcServer> _logger;

        public DemoRpcServer(ILogger<DemoRpcServer> logger)
        {
            _logger = logger;
        }

        public override async Task<Result> GetList(Input request, ServerCallContext context)
        {
            await Task.Yield();
            return Success("");
        }

        private Result Fail(string messages)
        {
            return new Result { Message = messages, Data = null, Success = false };
        }

        private Result Success(object data)
        {
            var byteStr = ByteString.CopyFrom(JsonSerializer.SerializeToUtf8Bytes(data));

            return new Result { Message = "成功!", Data = byteStr, Success = true };
        }
    }
}