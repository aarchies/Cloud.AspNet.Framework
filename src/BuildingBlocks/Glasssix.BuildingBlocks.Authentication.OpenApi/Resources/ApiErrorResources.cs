using Glasssix.BuildingBlocks.Authentication.OpenApi.ExceptionHandling;
using IdentityAuthServer.Admin.Api.Resources;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Resources
{
    public class ApiErrorResources : IApiErrorResources
    {
        public virtual ApiError CannotSetId()
        {
            return new ApiError
            {
                Code = nameof(CannotSetId),
                Description = ApiErrorResource.CannotSetId
            };
        }
    }
}