using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Users
{
    public class UserChangePasswordApiDto<TKey>
    {
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Password { get; set; }

        public TKey UserId { get; set; }
    }
}