namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.ApiResources
{
    public class ApiResourcesApiDto
    {
        public ApiResourcesApiDto()
        {
            ApiResources = new List<ApiResourceApiDto>();
        }

        public List<ApiResourceApiDto> ApiResources { get; set; }
        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}