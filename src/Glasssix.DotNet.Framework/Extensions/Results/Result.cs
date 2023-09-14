using Glasssix.Utils.MetaEntitys.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Glasssix.DotNet.Framework.Extensions.Results
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result : JsonResult, IResult
    {
        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result(StateCode code, string message, dynamic data = null) : base(null)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public StateCode Code { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        public bool Success { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            Value = new
            {
                Code = Code.Value(),
                Message,
                Data
            };
            return base.ExecuteResultAsync(context);
        }
    }
}