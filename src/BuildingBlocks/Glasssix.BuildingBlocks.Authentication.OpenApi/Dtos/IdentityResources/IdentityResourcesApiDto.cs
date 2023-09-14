namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.IdentityResources
{
    public class IdentityResourcesApiDto
    {
        public IdentityResourcesApiDto()
        {
            IdentityResources = new List<IdentityResourceApiDto>();
        }

        public List<IdentityResourceApiDto> IdentityResources { get; set; }
        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}