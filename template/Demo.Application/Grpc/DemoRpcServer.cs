using Demo.Application.Contracts;
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
        private readonly IDemoService _service;

        public DemoRpcServer(ILogger<DemoRpcServer> logger, IDemoService service)
        {
            _logger = logger;
            _service = service;
        }

        public override async Task<Result> GetList(Input request, ServerCallContext context)
        {
            var result = await _service.GetByIdAsync(request.Id);

            return Success(result);
        }

        private Result Success(object data)
        {
            var byteStr = ByteString.CopyFrom(JsonSerializer.SerializeToUtf8Bytes(data));

            return new Result { Message = "成功!", Data = byteStr, Success = true };
        }

        private Result Fail(string messages)
        {
            return new Result { Message = messages, Data = null, Success = false };
        }
    }
}
