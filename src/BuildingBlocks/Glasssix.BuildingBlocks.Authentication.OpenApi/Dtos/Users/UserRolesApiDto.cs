namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Users
{
    public class UserRolesApiDto<TRoleDto>
    {
        public UserRolesApiDto()
        {
            Roles = new List<TRoleDto>();
        }

        public int PageSize { get; set; }
        public List<TRoleDto> Roles { get; set; }
        public int TotalCount { get; set; }
    }
}