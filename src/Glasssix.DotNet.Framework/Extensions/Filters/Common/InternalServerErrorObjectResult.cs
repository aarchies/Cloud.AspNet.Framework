using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Glasssix.DotNet.Framework.Extensions.Filters.Common
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}