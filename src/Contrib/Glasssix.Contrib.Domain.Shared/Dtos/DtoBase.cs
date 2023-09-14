using System;

namespace Glasssix.Contrib.Domain.Shared.Dtos
{
    /// <summary>
    /// 数据传输对象
    /// </summary>
    [Serializable]
    public abstract class DtoBase : RequestBase, IDto
    {
        /// <summary>
        /// 标识
        /// </summary>
        public virtual long Id { get; set; }
    }
}