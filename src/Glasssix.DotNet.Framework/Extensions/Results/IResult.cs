namespace Glasssix.DotNet.Framework.Extensions.Results
{
    public interface IResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        StateCode Code { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        dynamic Data { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        bool Success { get; set; }
    }
}