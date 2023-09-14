using Glasssix.DotNet.Framework.Extensions.Filters.Common;
using Glasssix.DotNet.Framework.Extensions.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Glasssix.DotNet.Framework.Extensions.Filters.Attribute
{
    /// <summary>
    /// 全局接口请求日志
    /// </summary>
   // [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ApplictionRequestLogAttribute : ActionFilterAttribute
    {
        private readonly string header_requestbody_keyName = "api-requestInfo";
        private readonly string header_startTime_keyName = "api-starttime";

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            #region 计算执行耗时

            var request = context.HttpContext.Request;
            string startTime = request.Headers[header_startTime_keyName];
            double time = (DateTime.Now - Convert.ToDateTime(startTime)).TotalMilliseconds;
            request.Headers.Remove(header_startTime_keyName);

            #endregion 计算执行耗时

            #region 获取Action执行前封装的log

            string json_log = request.Headers[header_requestbody_keyName];
            request.Headers.Remove(header_requestbody_keyName);
            if (string.IsNullOrEmpty(json_log))
            {
                Log.Error($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}  未在header参数中找到key为{header_requestbody_keyName}的键");
            }
            else
            {
                RequestLog requestLog = JsonSerializer.Deserialize<RequestLog>(json_log, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                })!;

                if (requestLog == null)
                {
                    Log.Error($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}  反序列化 WebApplictionRequestLog 结果为 null\n");
                }
                else
                {
                    requestLog.Response = new Dictionary<string, object>() {
                        { "StatusCode", context.HttpContext.Response.StatusCode },
                        { "Time",time}
                    };
                    // 获取接口处理结果
                    if (context.Exception != null)
                    {
                        requestLog.Response.Add("IsException", true);
                        requestLog.Response.Add("Exception", context.Exception.Message);
                    }
                    else
                    {
                        var result = (context.Result as Result)?.Data;
                        requestLog.Response.Add("ResponseInfo", result);
                    }
                    string json_string = JsonSerializer.Serialize(requestLog, new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    });
                    Log.Debug("{0} {1}\n", DateTime.Now.ToString(), json_string);
                }
            }

            #endregion 获取Action执行前封装的log
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            CreateLog(context);
        }

        /// <summary>
        /// 创建log
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void CreateLog(ActionExecutingContext context)
        {
            string startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var request = context.HttpContext.Request;
            string requeststring = string.Empty;
            if (request.Method == "get")
            {
                requeststring = request.QueryString.Value;
            }
            else
            {
                var action_body = context.ActionArguments.SingleOrDefault(x => x.Key == "body").Value;
                if (action_body != null)
                {
                    requeststring = JsonSerializer.Serialize(action_body, new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    });
                    //requeststring = JsonSerializer.Serialize(action_body);
                }
            }
            RequestLog requestLog = new RequestLog()
            {
                Title = "接口追踪日志",
                ConnectId = context.HttpContext.Connection.Id,
                StartTime = startTime,
                RemoteIp = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                Headers = new Dictionary<string, string>() {
                    { "x-deviceid", request.Headers["x-deviceid"] },
                    { "x-appid", request.Headers["x-appid"] },
                    { "x-timestamp", request.Headers["x-timestamp"] },
                    { "x-data", request.Headers["x-data"] },
                    { "x-sign", request.Headers["x-sign"] }
                },
                Method = request.Method.ToLower(),
                UserAgent = request.Headers["User-Agent"],
                Path = request.Path,
                Route = $"{context.RouteData.Values["Controller"]}/{context.RouteData.Values["Action"]}",
                RequestInfo = requeststring
            };
            context.HttpContext.Request.Headers.Add(header_startTime_keyName, startTime);
            context.HttpContext.Request.Headers.Add(header_requestbody_keyName, JsonSerializer.Serialize(requestLog));
        }
    }
}