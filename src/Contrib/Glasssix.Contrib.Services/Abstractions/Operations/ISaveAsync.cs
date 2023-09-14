using Glasssix.BuildingBlocks.Data.Validations.Attributes;
using Glasssix.Contrib.Domain.Shared.Dtos;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Services.Abstractions.Operations
{
    /// <summary>
    /// 保存操作
    /// </summary>
    /// <typeparam name="TRequest">参数类型</typeparam>
    public interface ISaveAsync<in TRequest> where TRequest : IRequest, IKey, new()
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request">参数</param>
        Task SaveAsync([Valid] TRequest request);
    }
}