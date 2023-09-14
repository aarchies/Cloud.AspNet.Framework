using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Account
{
    public class RegisterViewModel
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

        [Required]
        public string UserName { get; set; }
    }
}