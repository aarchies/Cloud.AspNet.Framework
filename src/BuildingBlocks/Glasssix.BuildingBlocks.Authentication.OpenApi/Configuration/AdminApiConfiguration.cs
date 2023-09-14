namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Configuration
{
    public class AdminApiConfiguration
    {
        public string AdministrationRole { get; set; }
        public string ApiBaseUrl { get; set; }
        public string ApiName { get; set; }

        public string ApiVersion { get; set; }

        public bool CorsAllowAnyOrigin { get; set; }
        public string[] CorsAllowOrigins { get; set; }
        public string IdentityServerBaseUrl { get; set; }
        public string OidcApiName { get; set; }
        public string OidcSwaggerUIClientId { get; set; }

        public bool RequireHttpsMetadata { get; set; }
    }
}