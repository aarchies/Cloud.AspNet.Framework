using System;

namespace Glasssix.Contrib.Domain.Shared.Dtos
{
    /// <summary>
    /// 请求参数
    /// </summary>
    [Serializable]
    public abstract class RequestBase : IRequest
    {
        ///// <summary>
        ///// 验证
        ///// </summary>
        //public virtual ValidationResultCollection Validate()
        //{
        //    var result = DataAnnotationValidation.Validate(this);
        //    if (result.IsValid)
        //        return ValidationResultCollection.Success;
        //    throw new Exception(result.First().ErrorMessage);
        //}
    }
}