using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Account
{
    public class RegisterWithoutUsernameViewModel
    {
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}