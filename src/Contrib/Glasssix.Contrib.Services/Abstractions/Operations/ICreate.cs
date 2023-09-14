using Glasssix.BuildingBlocks.Data.Validations.Attributes;
using Glasssix.Contrib.Domain.Shared.Dtos;

namespace Glasssix.Contrib.Services.Abstractions.Operations
{
    /// <summary>
    /// 创建操作
    /// </summary>
    /// <typeparam name="TCreateRequest">创建参数类型</typeparam>
    public interface ICreate<in TCreateRequest> where TCreateRequest : IRequest, new()
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request">创建参数</param>
        //[AutoCommit]
        string Create([Valid] TCreateRequest request);
    }
}