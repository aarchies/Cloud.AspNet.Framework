using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Users
{
    public class UserClaimApiDto<TKey>
    {
        public int ClaimId { get; set; }

        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string ClaimValue { get; set; }

        public TKey UserId { get; set; }
    }
}