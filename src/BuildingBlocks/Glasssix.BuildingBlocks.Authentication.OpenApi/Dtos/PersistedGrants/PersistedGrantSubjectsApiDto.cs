namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.PersistedGrants
{
    public class PersistedGrantSubjectsApiDto
    {
        public PersistedGrantSubjectsApiDto()
        {
            PersistedGrants = new List<PersistedGrantSubjectApiDto>();
        }

        public int PageSize { get; set; }
        public List<PersistedGrantSubjectApiDto> PersistedGrants { get; set; }
        public int TotalCount { get; set; }
    }
}