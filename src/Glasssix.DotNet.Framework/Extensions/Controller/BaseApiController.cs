using Glasssix.DotNet.Framework.Extensions.Filters.Attribute;
using Glasssix.DotNet.Framework.Extensions.Filters.Authentication;
using Glasssix.DotNet.Framework.Extensions.Results;
using Glasssix.Utils.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Glasssix.DotNet.Framework.Extensions.Controller
{
    [ApiController]
    //[ApplictionRequestLog]
    [ValidateModelState]
    [HttpGlobalException]
    [TraceLog]
    [Route("api/v1/[controller]/[action]")]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        protected virtual IActionResult Fail(string message)
        {
            return new Result(StateCode.Fail, message);
        }

        /// <summary>
        /// 获取当前登录用户角色
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetRoleName() => HttpContext.User.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();

        /// <summary>
        /// 获取当前Token
        /// </summary>
        /// <returns></returns>
        protected virtual string GetToken() => HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult()!;

        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        /// <returns></returns>
        protected virtual string GetUserName() => HttpContext.User.FindFirstValue(ClaimTypes.Name);

        /// <summary>
        /// 使用账户密码进行登录
        /// </summary>
        /// <param name="identityUrl">STS服务器地址</param>
        /// <param name="clientId">客户端Id</param>
        /// <param name="scope">作用域</param>
        /// <param name="clientsecret">客户端密钥</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        protected virtual TokeResponse? Login(string identityUrl, string clientId, string scope, string clientsecret, string username, string password)
        {
            var response = RestSharpHelper.Post(identityUrl + "/connect/token", new
            {
                grant_type = "password",
                client_id = clientId,
                client_secret = clientsecret,
                username,
                password,
            });
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<TokeResponse>(response.Content!)!;
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        protected virtual IActionResult Success(dynamic? data = null, string? message = null)
        {
            message ??= "成功";
            return new Result(StateCode.Ok, message, data);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteStr"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual IActionResult Success<T>(object byteStr, string message = null)
        {
            return Success(byteStr, message);
        }
    }
}