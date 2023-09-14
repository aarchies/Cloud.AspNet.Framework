namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Users
{
    public class UserProviderApiDto<TKey>
    {
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
        public TKey UserId { get; set; }

        public string UserName { get; set; }
    }
}