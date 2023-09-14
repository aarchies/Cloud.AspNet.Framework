using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Roles
{
    public class RoleClaimApiDto<TKey>
    {
        public int ClaimId { get; set; }

        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string ClaimValue { get; set; }

        public TKey RoleId { get; set; }
    }
}