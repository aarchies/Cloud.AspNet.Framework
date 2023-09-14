using Glasssix.BuildingBlocks.Data.Mapping;
using Glasssix.BuildingBlocks.Data.Uow;
using Glasssix.Contrib.Domain;
using Glasssix.Contrib.Domain.Shared;
using Glasssix.Contrib.Domain.Shared.Dtos;
using Glasssix.Contrib.Repository.Base;
using Glasssix.Contrib.Services;
using Glasssix.Contrib.Services.Abstractions;
using Glasssix.Utils.MetaEntitys.Objects;

//using RedisCache.Client;

namespace Glasssix.Services
{
    /// <summary>
    /// 增删改查服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class SampleService<TEntity, TDto, TQueryParameter, TKey>
        : SampleService<TEntity, TDto, TDto, TQueryParameter, TKey>, ISampleService<TDto, TQueryParameter>
        where TEntity : class, IAggregateRoot<TEntity, TKey>, new()
        where TDto : IDto, new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly IRepository<TEntity, TKey> _repository;

        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        //public readonly RedisClient _redisClient = ServiceFactory.GetService<RedisClient>();
        /// <summary>
        /// 初始化增删改查服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">仓储</param>
        protected SampleService(IUnitOfWork unitOfWork, IRepository<TEntity, TKey> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
    }

    /// <summary>
    /// 增删改查服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TRequest">参数类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract class SampleService<TEntity, TDto, TRequest, TQueryParameter, TKey>
        : SampleService<TEntity, TDto, TRequest, TRequest, TRequest, TQueryParameter, TKey>, ISampleService<TDto, TRequest, TQueryParameter>
        where TEntity : class, IAggregateRoot<TEntity, TKey>, new()
        where TDto : IDto, new()
        where TRequest : IRequest, IKey, new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly IRepository<TEntity, TKey> _repository;

        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        //public readonly RedisClient _redisClient = ServiceFactory.GetService<RedisClient>();
        /// <summary>
        /// 初始化增删改查服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">仓储</param>
        protected SampleService(IUnitOfWork unitOfWork, IRepository<TEntity, TKey> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
    }

    /// <summary>
    /// 增删改查服务
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TRequest">参数类型</typeparam>
    /// <typeparam name="TCreateRequest">创建参数类型</typeparam>
    /// <typeparam name="TUpdateRequest">修改参数类型</typeparam>
    /// <typeparam name="TQueryParameter">查询参数类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public abstract partial class SampleService<TEntity, TDto, TRequest, TCreateRequest, TUpdateRequest, TQueryParameter, TKey>
        : DeleteService<TEntity, TDto, TQueryParameter, TKey>,
        ISampleService<TDto, TRequest, TCreateRequest, TUpdateRequest, TQueryParameter>
        where TEntity : class, IAggregateRoot<TEntity, TKey>, new()
        where TDto : IDto, new()
        where TRequest : IRequest, IKey, new()
        where TCreateRequest : IRequest, new()
        where TUpdateRequest : IRequest, new()
        where TQueryParameter : IQueryParameters
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly IRepository<TEntity, TKey> _repository;

        /// <summary>
        /// 工作单元
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        //public readonly RedisClient _redisClient = ServiceFactory.GetService<RedisClient>();
        /// <summary>
        /// 初始化增删改查服务
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="repository">仓储</param>
        protected SampleService(IUnitOfWork unitOfWork, IRepository<TEntity, TKey> repository) : base(unitOfWork, repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        /// <summary>
        /// 转换为实体
        /// </summary>
        /// <param name="request">参数</param>
        protected virtual TEntity ToEntity(TRequest request)
        {
            return request.MapTo<TEntity>();
        }

        /// <summary>
        /// 创建参数转换为实体
        /// </summary>
        /// <param name="request">创建参数</param>
        protected virtual TEntity ToEntityFromCreateRequest(TCreateRequest request)
        {
            if (typeof(TCreateRequest) == typeof(TRequest))
                return ToEntity(ObjectExtensions.To<TRequest>(request));
            return request.MapTo<TEntity>();
        }

        /// <summary>
        /// 参数转换为实体
        /// </summary>
        /// <param name="request">创建参数</param>
        protected virtual TEntity ToEntityFromDto(TDto request)
        {
            if (typeof(TDto) == typeof(TRequest))
                return ToEntity(ObjectExtensions.To<TRequest>(request));
            return request.MapTo<TEntity>();
        }

        /// <summary>
        /// 修改参数转换为实体
        /// </summary>
        /// <param name="request">修改参数</param>
        protected virtual TEntity ToEntityFromUpdateRequest(TUpdateRequest request)
        {
            if (typeof(TUpdateRequest) == typeof(TRequest))
                return ToEntity(ObjectExtensions.To<TRequest>(request));
            return request.MapTo<TEntity>();
        }
    }
}