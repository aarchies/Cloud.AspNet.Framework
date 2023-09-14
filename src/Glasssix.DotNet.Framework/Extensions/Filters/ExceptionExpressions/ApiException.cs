using System;

namespace Glasssix.DotNet.Framework.Extensions.Filters.ExceptionExpressions
{
    public class ApiException : Exception
    {
        public ApiException(ApiResponseStatus status, string msg = null, Exception exception = null) : base(msg, exception)
        {
            ExceptionCode = status;
        }

        public ApiResponseStatus ExceptionCode { get; set; }
    }
}