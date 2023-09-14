using DotXxlJob.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Scheduler.xxljob.Extensions
{
    public class XxlJobExecutorMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _provider;
        private readonly XxlRestfulServiceHandler _rpcService;

        public XxlJobExecutorMiddleware(IServiceProvider provider, RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _provider = provider;
            _next = next;
            _rpcService = _provider.GetRequiredService<XxlRestfulServiceHandler>();
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task Invoke(HttpContext context)
        {
            string contentType = context.Request.ContentType;

            if ("POST".Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(contentType)
                && contentType.ToLower().StartsWith("application/json"))
            {
                await _rpcService.HandlerAsync(context.Request, context.Response);

                return;
            }

            await _next.Invoke(context);
        }
    }
}