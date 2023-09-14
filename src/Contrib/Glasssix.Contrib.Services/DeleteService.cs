using Glasssix.BuildingBlocks.Data.Uow;
using Glasssix.Contrib.Domain;
using Glasssix.Contrib.Domain.Shared;
using Glasssix.Contrib.Domain.Shared.Dtos;
using Glasssix.Contrib.Repository;
using Glasssix.Contrib.Services.Abstractions;
using Glasssix.Utils.ReflectionConductor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Services
{
    /// <summary>
    /// 删除服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    public abstract class DeleteService<TEntity, TDto, TQueryParameter> : DeleteService<TEntity, TDto, TQueryParameter, string>
        where TEntity : class, IKey<string>, IAggregateRoot, new()
        where TDto : IDto, new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 初始化删除服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="store">存储器</param>
        protected DeleteService(IUnitOfWork unitOfWork, IStore<TEntity, string> store) : base(unitOfWork, store)
        {
        }
    }

    /// <summary>
    /// 删除服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class DeleteService<TEntity, TDto, TQueryParameter, TKey>
        : QueryService<TEntity, TDto, TQueryParameter, TKey>, IDeleteService<TDto, TQueryParameter>
        where TEntity : class, IKey<TKey>, IAggregateRoot, new()
        where TDto : new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 存储器
        /// </summary>
        private readonly IStore<TEntity, TKey> _store;

        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// 初始化删除服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="store">存储器</param>
        protected DeleteService(IUnitOfWork unitOfWork, IStore<TEntity, TKey> store) : base(store)
        {
            _unitOfWork = unitOfWork;
            _store = store;
            EntityDescription = Reflection.GetDisplayNameOrDescription<TEntity>();
        }

        /// <summary>
        /// 实体描述
        /// </summary>
        protected string EntityDescription { get; }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用逗号分隔的Id列表，范例："1,2"</param>
        public virtual async Task DeleteAsync(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return;
            var entities = await _store.FindByIdsAsync(ids);
            if (entities?.Count == 0)
                return;
            DeleteBefore(entities);
            await _store.RemoveAsync(entities);
            await _unitOfWork.CommitAsync();
            DeleteAfter(entities);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="entity">实体</param>
        protected void AddLog(TEntity entity)
        {
            //Log.BusinessId(entity.Id.SafeString());
            //Log.Content(entity.ToString());
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="entities">实体集合</param>
        protected void AddLog(IList<TEntity> entities)
        {
            //Log.BusinessId(entities.Select(t => t.Id).Join());
            //foreach (var entity in entities)
            //    Log.Content(entity.ToString());
        }

        /// <summary>
        /// 删除后操作
        /// </summary>
        /// <param name="entities">实体集合</param>
        protected virtual void DeleteAfter(List<TEntity> entities)
        {
            AddLog(entities);
            WriteLog($"删除{EntityDescription}成功");
        }

        /// <summary>
        /// 删除前操作
        /// </summary>
        /// <param name="entities">实体集合</param>
        protected virtual void DeleteBefore(List<TEntity> entities)
        {
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="caption">标题</param>
        protected void WriteLog(string caption)
        {
            //Log.Class(typeof(TEntity).FullName)
            //    .Caption(caption)
            //    .Info();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="entity">实体</param>
        protected void WriteLog(string caption, TEntity entity)
        {
            AddLog(entity);
            WriteLog(caption);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="entities">实体集合</param>
        protected void WriteLog(string caption, IList<TEntity> entities)
        {
            AddLog(entities);
            WriteLog(caption);
        }

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="ids">用逗号分隔的Id列表，范例："1,2"</param>
        //public virtual async void Delete(string ids)
        //{
        //    if (string.IsNullOrWhiteSpace(ids))
        //        return;
        //    var entities = _store.FindByIds(ids);
        //    if (entities?.Count == 0)
        //        return;
        //    DeleteBefore(entities);
        //    _store.Remove(entities);
        //    await _unitOfWork.CommitAsync();
        //    DeleteAfter(entities);
        //}
    }
}