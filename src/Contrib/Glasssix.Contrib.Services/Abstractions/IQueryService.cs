using Glasssix.Contrib.Domain.Shared;
using Glasssix.Contrib.Services.Abstractions.Queries;

namespace Glasssix.Contrib.Services.Abstractions
{
    /// <summary>
    /// 查询服务
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    public interface IQueryService<TDto, in TQueryParameter> : IService,
        IGetById<TDto>, IGetByIdAsync<TDto>,
        IGetAll<TDto>, IGetAllAsync<TDto>, IPageQuery<TDto, TQueryParameter>, IPageQueryAsync<TDto, TQueryParameter>
        where TDto : new()
        where TQueryParameter : IQueryParameters
    {
    }
}