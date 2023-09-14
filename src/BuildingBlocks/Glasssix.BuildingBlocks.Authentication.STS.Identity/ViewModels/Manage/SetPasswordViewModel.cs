using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Manage
{
    public class SetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}