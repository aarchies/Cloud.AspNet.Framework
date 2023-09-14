using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.ApiResources
{
    public class ApiResourceApiDto
    {
        public ApiResourceApiDto()
        {
            UserClaims = new List<string>();
            Scopes = new List<string>();
            AllowedAccessTokenSigningAlgorithms = new List<string>();
        }

        public List<string> AllowedAccessTokenSigningAlgorithms { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; } = true;
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<string> Scopes { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> UserClaims { get; set; }
    }
}