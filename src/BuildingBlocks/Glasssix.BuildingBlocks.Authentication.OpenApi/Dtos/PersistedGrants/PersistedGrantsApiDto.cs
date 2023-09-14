namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.PersistedGrants
{
    public class PersistedGrantsApiDto
    {
        public PersistedGrantsApiDto()
        {
            PersistedGrants = new List<PersistedGrantApiDto>();
        }

        public int PageSize { get; set; }
        public List<PersistedGrantApiDto> PersistedGrants { get; set; }
        public int TotalCount { get; set; }
    }
}