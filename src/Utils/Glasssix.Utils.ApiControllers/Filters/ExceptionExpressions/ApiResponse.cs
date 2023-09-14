namespace Glasssix.Utils.ApiControllers.Filters.ExceptionExpressions
{
    /// <summary>
    /// 接口输出基类
    /// </summary>
    [GenerateSerializer]
    public class ApiResponse
    {
        public ApiResponse(ApiResponseStatus _status, string _msg)
        {
            Code = _status;
            Message = _msg;
        }

        public ApiResponse(ApiResponseStatus _status, string _msg, object? _result = null)
        {
            Code = _status;
            Message = _msg;
            Result = _result;
        }

        /// <summary>
        /// 接口内部处理状态码
        /// </summary>
        public ApiResponseStatus Code { get; set; } = ApiResponseStatus.成功;

        /// <summary>
        /// 异常信息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 输出对象
        /// </summary>
        public object? Result { get; set; }
    }
}