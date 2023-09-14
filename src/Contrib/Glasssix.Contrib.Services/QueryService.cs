using Glasssix.BuildingBlocks.Data.Mapping;
using Glasssix.Contrib.Domain;
using Glasssix.Contrib.Domain.Shared;
using Glasssix.Contrib.Domain.Shared.Pager;
using Glasssix.Contrib.Repository;
using Glasssix.Contrib.Repository.Base;
using Glasssix.Contrib.Repository.Extensions;
using Glasssix.Contrib.Repository.Queries;
using Glasssix.Contrib.Services.Abstractions;
using Glasssix.Utils.MetaEntitys.Objects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Services
{
    /// <summary>
    /// 查询服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    public abstract class QueryService<TEntity, TDto, TQueryParameter> : QueryService<TEntity, TDto, TQueryParameter, string>
        where TEntity : class, IKey<string>
        where TDto : new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 初始化查询服务
        /// </summary>
        /// <param name="store">查询存储器</param>
        protected QueryService(IQueryStore<TEntity, string> store) : base(store)
        {
        }
    }

    /// <summary>
    /// 查询服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class QueryService<TEntity, TDto, TQueryParameter, TKey> : BaseService, IQueryService<TDto, TQueryParameter>
        where TEntity : class, IKey<TKey>
        where TDto : new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 查询存储器
        /// </summary>
        private readonly IQueryStore<TEntity, TKey> _store;

        /// <summary>
        /// Cache
        /// </summary>
        //public readonly RedisClient _redisClient = ServiceFactory.GetService<RedisClient>();

        /// <summary>
        /// 初始化查询服务
        /// </summary>
        /// <param name="store">查询存储器</param>
        protected QueryService(IQueryStore<TEntity, TKey> store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        #region GetAll

        /// <summary>
        /// 获取全部
        /// </summary>
        public virtual List<TDto> GetAll()
        {
            return _store.FindAll().Select(ToDto).ToList();
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        public virtual async Task<List<TDto>> GetAllAsync()
        {
            var entities = await _store.FindAllAsync();
            return entities.Select(ToDto).ToList();
        }

        #endregion GetAll

        #region GetById

        /// <summary>
        /// 通过编号获取
        /// </summary>
        /// <param name="id">实体编号</param>
        public virtual TDto GetById(object id)
        {
            var key = ObjectExtensions.To<TKey>(id);
            ////如果bloom不存在则直接跳过所有过程
            //if (!_redisClient._bloomFilter.Contains(Encoding.Default.GetBytes(key.ToString())))
            //{
            //    return default;
            //}
            //else
            //{
            //    //缓存存在取缓存 否则取数据库
            //    var isOn = _redisClient.ObjectGet<TDto>(key.ToString());
            //    if (isOn != null)
            //        return isOn;
            //}
            return ToDto(_store.Find(key));
        }

        /// <summary>
        /// 通过编号获取
        /// </summary>
        /// <param name="id">实体编号</param>
        public virtual async Task<TDto> GetByIdAsync(object id)
        {
            var key = ObjectExtensions.To<TKey>(id);

            //if (!await _redisClient._bloomFilter.ContainsAsync(Encoding.Default.GetBytes(key.ToString())))
            //{
            //    return default;
            //}
            //else
            //{
            //    var isOn = await _redisClient.ObjectGetAsync<TDto>(key.ToString());
            //    if (isOn != null)
            //        return isOn;
            //}

            return ToDto(await _store.FindAsync(key));
        }

        #endregion GetById

        #region GetByIds

        /// <summary>
        /// 通过编号列表获取
        /// </summary>
        /// <param name="ids">用逗号分隔的Id列表，范例："1,2"</param>
        public virtual List<TDto> GetByIds(string ids)
        {
            return _store.FindByIds(ids).Select(ToDto).ToList();
        }

        /// <summary>
        /// 通过编号列表获取
        /// </summary>
        /// <param name="ids"></param>
        public virtual List<TDto> GetByIds(long[] ids)
        {
            return _store.FindByIds(ids).Select(ToDto).ToList();
        }

        /// <summary>
        /// 通过编号列表获取
        /// </summary>
        /// <param name="ids">用逗号分隔的Id列表，范例："1,2"</param>
        public virtual async Task<List<TDto>> GetByIdsAsync(string ids)
        {
            var entities = await _store.FindByIdsAsync(ids);
            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// 通过编号列表获取
        /// </summary>
        /// <param name="ids"></param>
        public virtual async Task<List<TDto>> GetByIdsAsync(long[] ids)
        {
            var entities = await _store.FindByIdsAsync(ids);
            return entities.Select(ToDto).ToList();
        }

        #endregion GetByIds

        #region Query

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        public virtual List<TDto> Query(TQueryParameter parameter)
        {
            if (parameter == null)
                return new List<TDto>();

            //if (!_redisClient._bloomFilter.Contains(Encoding.Default.GetBytes(parameter.Id.ToString())))
            //{
            //    return default;
            //}
            //else
            //{
            //    var isOn = _redisClient.ObjectGet<TDto>(parameter.Id.ToString());
            //    if (isOn != null)
            //    {
            //        var result = new List<TDto>();
            //        result.Add(isOn);
            //        return result;
            //    }
            //}

            return ExecuteQuery(parameter).ToList().Select(ToDto).ToList();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        public virtual async Task<List<TDto>> QueryAsync(TQueryParameter parameter)
        {
            if (parameter == null)
                return new List<TDto>();

            //if (!await _redisClient._bloomFilter.ContainsAsync(Encoding.Default.GetBytes(parameter.Id.ToString())))
            //{
            //    return default;
            //}
            //else
            //{
            //    var isOn = await _redisClient.ObjectGetAsync<TDto>(parameter.Id.ToString());
            //    if (isOn != null)
            //    {
            //        var result = new List<TDto>();
            //        result.Add(isOn);
            //        return result;
            //    }
            //}

            return (await ExecuteQuery(parameter).ToListAsync()).Select(ToDto).ToList();
        }

        #endregion Query

        #region Execute

        /// <summary>
        /// 执行查询
        /// </summary>
        private IQueryable<TEntity> ExecuteQuery(TQueryParameter parameter)
        {
            var query = CreateQuery(parameter);
            var queryable = Filter(query);
            queryable = Filter(queryable, parameter);
            var order = query.GetOrder();
            return string.IsNullOrWhiteSpace(order) ? queryable : queryable.OrderBy<TEntity>(order);
        }

        #endregion Execute

        #region Common

        /// <summary>
        /// 查询时是否跟踪对象
        /// </summary>
        protected virtual bool IsTracking => false;

        /// <summary>
        /// 创建查询对象
        /// </summary>
        /// <param name="parameter">查询参数</param>
        protected virtual IQueryBase<TEntity> CreateQuery(TQueryParameter parameter)
        {
            return new Query<TEntity, TKey>(parameter);
        }

        /// <summary>
        /// 过滤
        /// </summary>
        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> queryable, TQueryParameter parameter)
        {
            return queryable;
        }

        /// <summary>
        /// 转换为数据传输对象
        /// </summary>
        /// <param name="entity">实体</param>
        protected virtual TDto ToDto(TEntity entity)
        {
            return entity.MapTo<TDto>();
        }

        /// <summary>
        /// 过滤
        /// </summary>
        private IQueryable<TEntity> Filter(IQueryBase<TEntity> query)
        {
            return IsTracking ? _store.Find().Where(query) : _store.FindAsNoTracking().Where(query);
        }

        #endregion Common

        #region PageQuery

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        public virtual PagerList<TDto> PagerQuery(TQueryParameter parameter)
        {
            if (parameter == null)
                return new PagerList<TDto>();
            var query = CreateQuery(parameter);
            var queryable = Filter(query);
            queryable = Filter(queryable, parameter);
            return queryable.ToPagerList(query.GetPager()).Convert(ToDto);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="parameter">查询参数</param>
        public virtual async Task<PagerList<TDto>> PagerQueryAsync(TQueryParameter parameter)
        {
            if (parameter == null)
                return new PagerList<TDto>();
            var query = CreateQuery(parameter);
            var queryable = Filter(query);
            queryable = Filter(queryable, parameter);
            return (await queryable.ToPagerListAsync(query.GetPager())).Convert(ToDto);
        }

        #endregion PageQuery
    }
}