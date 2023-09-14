using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Glasssix.Utils.ApiControllers.Filters
{
    /// <summary> 
    /// 隐藏接口，不生成到swagger文档展示 
    /// </summary> 
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HiddenApiAttribute : System.Attribute
    {


    }


    public abstract class SwaggerIgnoreFilter : IDocumentFilter
    {
        public abstract void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context);

    }

}
