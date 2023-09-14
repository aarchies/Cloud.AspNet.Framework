using System.Collections.Generic;

namespace Glasssix.DotNet.Framework.Extensions.Filters.Common
{
    public class RequestLog
    {
        /// <summary>
        /// 连接标识
        /// </summary>
        public string ConnectId { get; set; } = string.Empty;

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        ///// <summary>
        ///// 应用ID （业务参数）
        ///// </summary>
        //public string AppId { get; set; } = string.Empty;
        ///// <summary>
        ///// 设备标识（业务参数）
        ///// </summary>
        //public string DeviceId { get; set; } = string.Empty;
        /// <summary>
        /// http 请求类型
        /// </summary>
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 远程客户端IP
        /// </summary>
        public string RemoteIp { get; set; } = string.Empty;

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestInfo { get; set; } = string.Empty;

        public Dictionary<string, object> Response { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 接口路由
        /// </summary>
        public string Route { get; set; } = string.Empty;

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; } = string.Empty;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;
    }
}