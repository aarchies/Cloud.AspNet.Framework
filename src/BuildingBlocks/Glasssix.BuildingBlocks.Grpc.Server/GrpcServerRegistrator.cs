using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glasssix.BuildingBlocks.Grpc.Server
{
    public class GrpcServerRegistrator
    {
        private readonly ILogger<GrpcServerRegistrator> _logger;

        public GrpcServerRegistrator(ILogger<GrpcServerRegistrator> logger)
        {
            _logger = logger;
        }

        private IEnumerable<Type>? _types { get; set; }

        public IEnumerable<Type> GetEntries()
        {
            _types = AppDomain.CurrentDomain.GetAssemblies().Where(i => i.IsDynamic == false)
                .SelectMany(i => i.ExportedTypes).ToArray();
            var services = _types.Where(i =>
            {
                var typeInfo = i.GetTypeInfo();
                return typeInfo.IsClass && typeInfo.GetCustomAttribute<GrpcServerAttribute>() != null;
            }).ToArray();
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogDebug($"发现 {services.Length} 个GRPC服务：\t[ {string.Join(",", services.Select(i => i.Name.ToString()))} ]\t");
            }
            return services;
        }
    }
}