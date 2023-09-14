namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Users
{
    public class UserRoleApiDto<TKey>
    {
        public TKey RoleId { get; set; }
        public TKey UserId { get; set; }
    }
}