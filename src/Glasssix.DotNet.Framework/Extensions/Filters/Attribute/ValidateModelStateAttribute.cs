using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Glasssix.DotNet.Framework.Extensions.Filters.Attribute
{
    /// <summary>
    /// 模型验证过滤器
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var validationErrors = context.ModelState
                .Keys
                .SelectMany(k => context.ModelState[k].Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            var json = new JsonErrorResponse
            {
                Messages = validationErrors
            };

            context.Result = new BadRequestObjectResult(json);
        }
    }
}