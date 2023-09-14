﻿using Glasssix.Utils.MetaEntitys.Objects;
using Glasssix.Utils.MetaEntitys.Strings;
using System;

namespace Glasssix.Contrib.Domain.Auditing
{
    /// <summary>
    /// 修改操作审计初始化器
    /// </summary>
    public class ModificationAuditedInitializer
    {
        /// <summary>
        /// 实体
        /// </summary>
        private readonly object _entity;

        /// <summary>
        /// 用户标识
        /// </summary>
        private readonly string _userId;

        /// <summary>
        /// 用户名称
        /// </summary>
        private readonly string _userName;

        /// <summary>
        /// 初始化修改操作审计初始化器
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userId">用户标识</param>
        /// <param name="userName">用户名称</param>
        private ModificationAuditedInitializer(object entity, string userId, string userName)
        {
            _entity = entity;
            _userId = userId;
            _userName = userName;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userId">用户标识</param>
        /// <param name="userName">用户名称</param>
        public static void Init(object entity, string userId, string userName)
        {
            new ModificationAuditedInitializer(entity, userId, userName).Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (_entity == null)
                return;
            InitLastModificationTime();
            InitModifier();
            if (string.IsNullOrWhiteSpace(_userId))
                return;
            switch (_entity)
            {
                case IModificationAudited<Guid> _:
                    InitGuid();
                    return;

                case IModificationAudited<Guid?> _:
                    InitNullableGuid();
                    return;

                case IModificationAudited<int> _:
                    InitInt();
                    return;

                case IModificationAudited<int?> _:
                    InitNullableInt();
                    return;

                case IModificationAudited<string> _:
                    InitString();
                    return;

                case IModificationAudited<long> _:
                    InitLong();
                    return;

                case IModificationAudited<long?> _:
                    InitNullableLong();
                    break;
            }
        }

        /// <summary>
        /// 初始化Guid
        /// </summary>
        private void InitGuid()
        {
            var result = (IModificationAudited<Guid>)_entity;
            result.LastModifierId = _userId.ToGuid();
        }

        /// <summary>
        /// 初始化int
        /// </summary>
        private void InitInt()
        {
            var result = (IModificationAudited<int>)_entity;
            result.LastModifierId = _userId.ToInt();
        }

        /// <summary>
        /// 初始化最后修改时间
        /// </summary>
        private void InitLastModificationTime()
        {
            if (_entity is IModificationTime result)
                result.LastModificationTime = DateTime.Now;
        }

        /// <summary>
        /// 初始化Long
        /// </summary>
        private void InitLong()
        {
            var result = (IModificationAudited<long>)_entity;
            result.LastModifierId = _userId.ToLong();
        }

        /// <summary>
        /// 初始化修改人
        /// </summary>
        private void InitModifier()
        {
            if (string.IsNullOrWhiteSpace(_userName))
                return;
            if (_entity is IModifier result)
                result.Modifier = _userName;
        }

        /// <summary>
        /// 初始化可空Guid
        /// </summary>
        private void InitNullableGuid()
        {
            var result = (IModificationAudited<Guid?>)_entity;
            result.LastModifierId = _userId.ToGuidOrNull();
        }

        /// <summary>
        /// 初始化可空int
        /// </summary>
        private void InitNullableInt()
        {
            var result = (IModificationAudited<int?>)_entity;
            result.LastModifierId = _userId.ToIntOrNull();
        }

        /// <summary>
        /// 初始化可空Long
        /// </summary>
        private void InitNullableLong()
        {
            var result = (IModificationAudited<long?>)_entity;
            result.LastModifierId = _userId.ToLongOrNull();
        }

        /// <summary>
        /// 初始化字符串
        /// </summary>
        private void InitString()
        {
            var result = (IModificationAudited<string>)_entity;
            result.LastModifierId = _userId.SafeString();
        }
    }
}