using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Manage
{
    public class EnableAuthenticatorViewModel
    {
        [BindNever]
        public string AuthenticatorUri { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }

        [BindNever]
        public string SharedKey { get; set; }
    }
}