using Glasssix.BuildingBlocks.Data.Validations.Attributes;
using Glasssix.Contrib.Domain.Shared.Dtos;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Services.Abstractions.Operations
{
    /// <summary>
    /// 修改操作
    /// </summary>
    /// <typeparam name="TUpdateRequest">修改参数类型</typeparam>
    public interface IUpdateAsync<in TUpdateRequest> where TUpdateRequest : IRequest, new()
    {
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="request">修改参数</param>
        Task UpdateAsync([Valid] TUpdateRequest request);
    }
}