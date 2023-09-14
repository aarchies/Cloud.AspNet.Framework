using Glasssix.Contrib.Domain.Shared;
using Glasssix.Contrib.Domain.Shared.Pager;
using System.Collections.Generic;

namespace Glasssix.Contrib.Services.Abstractions.Queries
{
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    public interface IPageQuery<TDto, in TQueryParameter>
        where TDto : new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        PagerList<TDto> PagerQuery(TQueryParameter parameter);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        List<TDto> Query(TQueryParameter parameter);
    }
}