namespace Glasssix.DotNet.Framework.Extensions.Filters
{
    /// <summary>
    /// 全局异常消息模板
    /// </summary>
    public class JsonErrorResponse
    {
        /// <summary>
        /// 开发环境异常输出
        /// </summary>
        public object DeveloperMessage { get; set; }

        /// <summary>
        /// 错误输出消息
        /// </summary>
        public string[] Messages { get; set; }
    }
}