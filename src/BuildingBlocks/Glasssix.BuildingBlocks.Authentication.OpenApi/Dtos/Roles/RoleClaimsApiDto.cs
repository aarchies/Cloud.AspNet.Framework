namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Roles
{
    public class RoleClaimsApiDto<TKey>
    {
        public RoleClaimsApiDto()
        {
            Claims = new List<RoleClaimApiDto<TKey>>();
        }

        public List<RoleClaimApiDto<TKey>> Claims { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}