using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Manage
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}