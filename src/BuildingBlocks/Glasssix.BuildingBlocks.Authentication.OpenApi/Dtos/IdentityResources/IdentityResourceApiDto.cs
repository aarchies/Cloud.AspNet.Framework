using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.IdentityResources
{
    public class IdentityResourceApiDto
    {
        public IdentityResourceApiDto()
        {
            UserClaims = new List<string>();
        }

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