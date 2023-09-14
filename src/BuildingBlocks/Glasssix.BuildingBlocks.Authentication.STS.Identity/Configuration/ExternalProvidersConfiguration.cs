namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.Configuration
{
    public class ExternalProvidersConfiguration
    {
        public string AzureAdCallbackPath { get; set; }
        public string AzureAdClientId { get; set; }
        public string AzureAdSecret { get; set; }
        public string AzureAdTenantId { get; set; }
        public string AzureDomain { get; set; }
        public string AzureInstance { get; set; }
        public string GitHubCallbackPath { get; set; }
        public string GitHubClientId { get; set; }
        public string GitHubClientSecret { get; set; }
        public bool UseAzureAdProvider { get; set; }
        public bool UseGitHubProvider { get; set; }
    }
}