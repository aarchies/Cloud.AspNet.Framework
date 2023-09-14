using Glasssix.Contrib.Domain;
using Glasssix.Contrib.Domain.Shared.Pager;
using Glasssix.Contrib.Repository.Base;
using System.Collections.Generic;

namespace Glasssix.Contrib.Repository.Operations
{
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="TEntity">对象类型</typeparam>
    /// <typeparam name="TKey">对象标识类型</typeparam>
    public interface IPageQuery<TEntity, in TKey> where TEntity : class, IKey<TKey>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询对象</param>
        PagerList<TEntity> PagerQuery(IQueryBase<TEntity> query);

        /// <summary>
        /// 分页查询，不跟踪实体
        /// </summary>
        /// <param name="query">查询对象</param>
        PagerList<TEntity> PagerQueryAsNoTracking(IQueryBase<TEntity> query);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">查询对象</param>
        List<TEntity> Query(IQueryBase<TEntity> query);

        /// <summary>
        /// 查询，不跟踪实体
        /// </summary>
        /// <param name="query">查询对象</param>
        List<TEntity> QueryAsNoTracking(IQueryBase<TEntity> query);
    }
}