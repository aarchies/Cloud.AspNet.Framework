using System;

namespace Glasssix.BuildingBlocks.Grpc.Server
{
    /// <summary>
    /// GRpc服务集标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GrpcServerAttribute : Attribute
    {
    }
}