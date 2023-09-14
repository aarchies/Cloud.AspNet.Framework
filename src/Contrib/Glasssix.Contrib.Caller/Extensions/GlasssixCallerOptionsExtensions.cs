using Glasssix.Contrib.Caller.Infrastructure.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Glasssix.Contrib.Caller.Extensions
{
    public static class GlasssixCallerOptionsExtensions
    {
        /// <summary>
        /// 指定的调用者设置请求处理程序和响应处理程序
        /// </summary>
        /// <param name="GlasssixCallerOptions"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public static GlasssixCallerClient UseJson(this GlasssixCallerClient GlasssixCallerOptions, JsonSerializerOptions? jsonSerializerOptions)
        {
            GlasssixCallerOptions.RequestMessageFactory = _ => new JsonRequestMessage(jsonSerializerOptions);
            GlasssixCallerOptions.ResponseMessageFactory = serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                return new JsonResponseMessage(jsonSerializerOptions, loggerFactory);
            };
            return GlasssixCallerOptions;
        }
    }
}