using Glasssix.DotNet.Framework.Extensions.Filters.ExceptionExpressions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;

namespace Glasssix.DotNet.Framework.Extensions.Filters.Attribute
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class HttpGlobalExceptionAttribute : ExceptionFilterAttribute
    {
        //    Log.Error("{eid},{e},{message}",
        //             "全局异常 => ",
        //                  context.Exception.Message,
        //                  context.Exception);

        //        var message = context.Exception.Message;
        //    var value = new object { };

        //        if (GlasssixIocApp.TryGetEnvironment().IsDevelopment())
        //        {
        //            value = context.Exception;
        //        }

        //var result = new Result(StateCode.Fail, message)
        //{
        //    StatusCode = 500,
        //    Value = value,
        //};

        //context.Result = new JsonResult(result);

        //context.ExceptionHandled = true;

        /// <summary>
        /// 重写事件
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            Log.Error("{eid},{e},{message}",
                 "全局异常 => ",
                      context.Exception.Message,
                      context.Exception);

            ApiResponseStatus code = ApiResponseStatus.异常;
            if (context.Exception.GetType() == typeof(ApiException))
            {
                ApiException exception = context.Exception as ApiException ??
                                new ApiException(ApiResponseStatus.异常, context.Exception.Message, context.Exception);
                code = exception.ExceptionCode;
            }
            string content = JsonConvert.SerializeObject(new ApiResponse(code, context.Exception.Message, ""));
            context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(content);
        }
    }
}