using Glasssix.Contrib.Domain.Shared;
using Glasssix.Contrib.Domain.Shared.Dtos;
using Glasssix.Contrib.Services.Abstractions;
using Glasssix.Contrib.Services.Extensions;
using Glasssix.Utils.MetaEntitys.Json;
using Glasssix.Utils.MetaEntitys.Strings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glasssix.DotNet.Framework.Extensions.Controller
{
    /// <summary>
    /// Crud控制器
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TQuery">查询参数类型</typeparam>
    public abstract class SampleController<TDto, TQuery> : SampleController<TDto, TDto, TDto, TQuery>
        where TQuery : IQueryParameters
        where TDto : IDto, new()
    {
        /// <summary>
        /// 初始化Crud控制器
        /// </summary>
        /// <param name="service">Crud服务</param>
        protected SampleController(ISampleService<TDto, TQuery> service)
            : base(service)
        {
        }
    }

    /// <summary>
    /// Crud控制器
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TRequest">参数类型</typeparam>
    /// <typeparam name="TQuery">查询参数类型</typeparam>
    public abstract class SampleController<TDto, TRequest, TQuery> : SampleController<TDto, TRequest, TRequest, TQuery>
        where TQuery : IQueryParameters
        where TRequest : IRequest, IKey, new()
        where TDto : IDto, new()
    {
        /// <summary>
        /// 初始化Crud控制器
        /// </summary>
        /// <param name="service">Crud服务</param>
        protected SampleController(ISampleService<TDto, TRequest, TQuery> service)
            : base(service)
        {
        }
    }

    /// <summary>
    /// Crud控制器
    /// </summary>
    /// <typeparam name="TDto">数据传输对象类型</typeparam>
    /// <typeparam name="TCreateRequest">创建参数类型</typeparam>
    /// <typeparam name="TUpdateRequest">修改参数类型</typeparam>
    /// <typeparam name="TQuery">查询参数类型</typeparam>
    public abstract class SampleController<TDto, TCreateRequest, TUpdateRequest, TQuery> : QueryController<TDto, TQuery>
        where TQuery : IQueryParameters
        where TCreateRequest : IRequest, new()
        where TUpdateRequest : IRequest, IKey, new()
        where TDto : IDto, new()
    {
        /// <summary>
        /// Crud服务
        /// </summary>
        private readonly ISampleService<TDto, TUpdateRequest, TCreateRequest, TUpdateRequest, TQuery> _service;

        /// <summary>
        /// 初始化Crud控制器
        /// </summary>
        /// <param name="service">Crud服务</param>
        protected SampleController(ISampleService<TDto, TUpdateRequest, TCreateRequest, TUpdateRequest, TQuery> service)
            : base(service)
        {
            _service = service;
        }

        /// <summary>
        /// 批量删除，注意：body参数需要添加引号，"'1,2,3'"而不是"1,2,3"
        /// </summary>
        /// <remarks>
        /// 调用范例:
        /// POST
        /// /api/customer/delete
        /// body: "'1,2,3'"
        /// </remarks>
        /// <param name="ids">标识列表，多个Id用逗号分隔，范例：1,2,3</param>
        [HttpPost("delete")]
        public virtual async Task<IActionResult> BatchAsync([FromBody] string ids)
        {
            await _service.DeleteAsync(ids);
            return Success();
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="request">保存参数</param>
        [HttpPost("save")]
        public virtual async Task<IActionResult> BatchAsync([FromBody] SaveModel request)
        {
            if (request == null)
                return Fail("参数不能为空");
            var creationList = Json.ToObject<List<TDto>>(request.CreationList);
            var updateList = Json.ToObject<List<TDto>>(request.UpdateList);
            var deleteList = Json.ToObject<List<TDto>>(request.DeleteList);
            await _service.SaveAsync(creationList, updateList, deleteList);
            return Success();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// 调用范例:
        /// POST
        /// /api/customer
        /// </remarks>
        /// <param name="request">创建参数</param>
        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync([FromBody] TCreateRequest request)
        {
            if (request == null)
                return Fail("创建参数不能为空");
            CreateBefore(request);
            var id = await _service.CreateAsync(request);
            var result = await _service.GetByIdAsync(id);
            return Success(result);
        }

        /// <summary>
        /// 删除，注意：该方法用于删除单个实体，批量删除请使用POST提交，否则可能失败
        /// </summary>
        /// <remarks>
        /// 调用范例:
        /// DELETE
        /// /api/customer/1
        /// </remarks>
        /// <param name="id">标识</param>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(string id)
        {
            await _service.DeleteAsync(id);
            return Success();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <remarks>
        /// 调用范例:
        /// PUT
        /// /api/customer/1
        /// </remarks>
        /// <param name="id">标识</param>
        /// <param name="request">修改参数</param>
        [HttpPut("{id?}")]
        public virtual async Task<IActionResult> UpdateAsync(string id, [FromBody] TUpdateRequest request)
        {
            if (request == null)
                return Fail("修改参数不能为空");
            if (id.IsEmpty() && request.Id == 0)
                return Fail("Id不能为空");
            if (request.Id == 0)
                request.Id = Convert.ToInt32(id);
            UpdateBefore(request);
            await _service.UpdateAsync(request);
            var result = await _service.GetByIdAsync(request.Id);
            return Success(result);
        }

        /// <summary>
        /// 创建前操作
        /// </summary>
        /// <param name="dto">创建参数</param>
        protected virtual void CreateBefore(TCreateRequest dto)
        {
        }

        /// <summary>
        /// 修改前操作
        /// </summary>
        /// <param name="dto">修改参数</param>
        protected virtual void UpdateBefore(TUpdateRequest dto)
        {
        }
    }
}