using Glasssix.Contrib.Domain.Implements;
using System;
using System.Threading.Tasks;

namespace Glasssix.Services
{
    /// <summary>
    /// 增删改查服务 - Save
    /// </summary>
    public abstract partial class SampleService<TEntity, TDto, TRequest, TCreateRequest, TUpdateRequest, TQueryParameter, TKey>
    {
        #region Create

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request">创建参数</param>
        public virtual string Create(TCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var entity = ToEntityFromCreateRequest(request);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Create(entity);
            return entity.Id.ToString();
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        protected void Create(TEntity entity)
        {
            CreateBefore(entity);
            entity.Init();
            _repository.Add(entity);
            _unitOfWork.CommitAsync().GetAwaiter().GetResult();
            CreateAfter(entity);
        }

        /// <summary>
        /// 创建后操作
        /// </summary>
        protected virtual void CreateAfter(TEntity entity)
        {
            AddLog(entity);
        }

        /// <summary>
        /// 创建前操作
        /// </summary>
        protected virtual void CreateBefore(TEntity entity)
        {
        }

        #endregion Create

        #region CreateAsync

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request">创建参数</param>
        public virtual async Task<string> CreateAsync(TCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var entity = ToEntityFromCreateRequest(request);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await CreateAsync(entity);
            //await this._redisClient.BlockingWork(entity.Id.ToString(), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), () => { return SetCache(entity); });
            return entity.Id.ToString();
        }

        /// <summary>
        /// 创建后操作
        /// </summary>
        protected virtual Task CreateAfterAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 创建实体
        /// </summary>
        protected async Task CreateAsync(TEntity entity)
        {
            await CreateBeforeAsync(entity);
            entity.Init();
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            entity.Validate();
            await CreateAfterAsync(entity);
        }

        /// <summary>
        /// 创建前操作
        /// </summary>
        protected virtual Task CreateBeforeAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }

        #endregion CreateAsync

        #region Update

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="request">修改参数</param>
        public virtual void Update(TUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var entity = ToEntityFromUpdateRequest(request);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Update(entity);
        }

        /// <summary>
        /// 查找旧实体
        /// </summary>
        /// <param name="id">标识</param>
        protected virtual TEntity FindOldEntity(TKey id)
        {
            return _repository.Find(id);
        }

        /// <summary>
        /// 查找旧实体
        /// </summary>
        /// <param name="id">标识</param>
        protected virtual async Task<TEntity> FindOldEntityAsync(TKey id)
        {
            return await _repository.FindAsync(id);
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        protected void Update(TEntity entity)
        {
            var oldEntity = FindOldEntity(entity.Id);
            if (oldEntity == null)
                throw new ArgumentNullException(nameof(oldEntity));
            var changes = oldEntity.GetChanges(entity);
            UpdateBefore(entity);
            _repository.Update(entity);
            _unitOfWork.CommitAsync().GetAwaiter().GetResult();
            UpdateAfter(entity, changes);
        }

        /// <summary>
        /// 修改后操作
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="changeValues">变更值集合</param>
        protected virtual void UpdateAfter(TEntity entity, ChangeValueCollection changeValues)
        {
            //Log.BusinessId(entity.Id.SafeString()).Content($"Id:{entity.Id},{changeValues}");
        }

        /// <summary>
        /// 修改前操作
        /// </summary>
        /// <param name="entity">实体</param>
        protected virtual void UpdateBefore(TEntity entity)
        {
        }

        #endregion Update

        #region UpdateAsync

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="request">修改参数</param>
        public virtual async Task UpdateAsync(TUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var entity = ToEntityFromUpdateRequest(request);//id值被改变
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await UpdateAsync(entity);
        }

        /// <summary>
        /// 修改后操作
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="changeValues">变更值集合</param>
        protected virtual Task UpdateAfterAsync(TEntity entity, ChangeValueCollection changeValues)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        protected async Task UpdateAsync(TEntity entity)
        {
            var oldEntity = await FindOldEntityAsync(entity.Id);
            if (oldEntity == null)
                throw new ArgumentNullException(nameof(oldEntity));
            var changes = oldEntity.GetChanges(entity);
            await UpdateBeforeAsync(entity);
            await _repository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();
            //await this._redisClient.BlockingWork(entity.Id.ToString(), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), () => { return SetCache(entity); });
            await UpdateAfterAsync(entity, changes);
        }

        /// <summary>
        /// 修改前操作
        /// </summary>
        /// <param name="entity">实体</param>
        protected virtual Task UpdateBeforeAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }

        #endregion UpdateAsync

        #region IsCreate

        /// <summary>
        /// 是否创建
        /// </summary>
        /// <param name="request">参数</param>
        /// <param name="entity">实体</param>
        protected virtual bool IsNew(TRequest request, TEntity entity)
        {
            return string.IsNullOrWhiteSpace(request.Id.ToString()) || entity.Id.Equals(default(TKey));
        }

        #endregion IsCreate

        #region Save

        /// <summary>
        /// 提交后操作 - 该方法由工作单元拦截器调用
        /// </summary>
        public void CommitAfter()
        {
            SaveAfter();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="request">参数</param>
        public virtual async Task SaveAsync(TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            SaveBefore(request);
            var entity = ToEntity(request);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (IsNew(request, entity))
            {
                await CreateAsync(entity);
                request.Id = Convert.ToInt64(entity.Id);
            }
            else
                await UpdateAsync(entity);
        }

        /// <summary>
        /// 保存后操作
        /// </summary>
        protected virtual void SaveAfter()
        {
            WriteLog($"保存{EntityDescription}成功");
        }

        /// <summary>
        /// 保存前操作
        /// </summary>
        /// <param name="request">参数</param>
        protected virtual void SaveBefore(TRequest request)
        {
        }

        #endregion Save

        #region Cache

        //private Task SetCache(TEntity entity)
        //{
        //    return this._redisClient.ObjectSetAsync<object>($"Application:{entity.GetType().Name}:{entity.Id.ToString()}", entity);
        //}

        #endregion Cache
    }
}