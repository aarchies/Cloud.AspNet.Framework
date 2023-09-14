using System;

namespace Glasssix.DotNet.Framework.Extensions.Filters.ExceptionExpressions
{
    public class BusinessException : Exception
    {
        public BusinessException(string msg) : base(msg)
        {
        }
    }
}