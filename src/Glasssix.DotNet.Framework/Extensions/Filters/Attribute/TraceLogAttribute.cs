using Glasssix.Contrib.EventBus.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Glasssix.DotNet.Framework.Extensions.Filters.Attribute
{
    /// <summary>
    /// 跟踪日志过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TraceLogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 日志名
        /// </summary>
        public const string TraceLogName = "ApiTraceLog";

        /// <summary>
        /// 是否忽略,为true不记录日志
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            Log.Debug("Http Requesting {id} {Controller} {ActionArguments} {Method} {Path}", $" ActionId:{context.ActionDescriptor.Id}", $"Controller: {context.Controller.GetGenericTypeName()}", $"ActionArguments:{string.Join(",", context.ActionArguments.Select(x => x.Key + ":" + x.Value))}", $"Method:{context.HttpContext.Request.Method}", $"Path:{context.HttpContext.Request.Path}");
            OnActionExecuting(context);

            if (context.Result != null)
                return;

            OnActionExecuted(await next());
            Log.Debug("Http Requested {id} {Controller} Completed!", $"ActionId:{context.ActionDescriptor.Id}", $"Controller: {context.Controller.GetGenericTypeName()}");
        }
    }
}