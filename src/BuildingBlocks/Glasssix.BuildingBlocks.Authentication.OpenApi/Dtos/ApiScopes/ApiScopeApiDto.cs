using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.ApiScopes
{
    public class ApiScopeApiDto
    {
        public ApiScopeApiDto()
        {
            UserClaims = new List<string>();
        }

        public List<ApiScopePropertyApiDto> ApiScopeProperties { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Emphasize { get; set; }
        public bool Enabled { get; set; } = true;
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<string> UserClaims { get; set; }
    }
}