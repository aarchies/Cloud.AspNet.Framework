namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Users
{
    public class UserProviderDeleteApiDto<TKey>
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public TKey UserId { get; set; }
    }
}