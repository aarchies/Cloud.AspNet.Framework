using Glasssix.Utils.ApiControllers.Filters.ExceptionExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Glasssix.Utils.ApiControllers.Filters.Attribute
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class HttpGlobalExceptionAttribute : ExceptionFilterAttribute
    {

        /// <summary>
        /// 重写事件
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            Console.WriteLine(context.Exception.Message);

            ApiResponseStatus code = ApiResponseStatus.异常;
            if (context.Exception.GetType() == typeof(ApiException))
            {
                ApiException exception = context.Exception as ApiException ??
                                new ApiException(ApiResponseStatus.异常, context.Exception.Message, context.Exception);
                code = exception.ExceptionCode;
            }

            context.Result = new JsonResult(new ApiResponse(code, context.Exception.Message, ""));
        }
    }
}