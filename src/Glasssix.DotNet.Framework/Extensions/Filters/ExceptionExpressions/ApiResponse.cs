namespace Glasssix.DotNet.Framework.Extensions.Filters.ExceptionExpressions
{
    /// <summary>
    /// 接口输出基类
    /// </summary>
    public class ApiResponse
    {
        public ApiResponse(ApiResponseStatus _status, string _errorMsg, object _result)
        {
            status = _status;
            errorMsg = _errorMsg;
            result = _result;
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string errorMsg { get; set; } = "";

        /// <summary>
        /// 输出对象
        /// </summary>
        public object result { get; set; } = new object();

        /// <summary>
        /// 接口内部处理状态码
        /// </summary>
        public ApiResponseStatus status { get; set; } = ApiResponseStatus.成功;
    }

    /// <summary>
    /// 接口输出基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        public ApiResponse(ApiResponseStatus _status, string _errorMsg, T _result)
        {
            status = _status;
            errorMsg = _errorMsg;
            result = _result;
        }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string errorMsg { get; set; } = "";

        /// <summary>
        /// 输出对象
        /// </summary>
        public T result { get; set; } = default;

        /// <summary>
        /// 接口内部处理状态码
        /// </summary>
        public ApiResponseStatus status { get; set; } = ApiResponseStatus.成功;
    }
}