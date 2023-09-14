namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.IdentityResources
{
    public class IdentityResourcePropertiesApiDto
    {
        public IdentityResourcePropertiesApiDto()
        {
            IdentityResourceProperties = new List<IdentityResourcePropertyApiDto>();
        }

        public List<IdentityResourcePropertyApiDto> IdentityResourceProperties { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}