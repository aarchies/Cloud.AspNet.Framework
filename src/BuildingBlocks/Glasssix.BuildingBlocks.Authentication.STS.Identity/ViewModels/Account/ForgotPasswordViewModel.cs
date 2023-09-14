using Skoruba.IdentityServer4.Shared.Configuration.Configuration.Identity;
using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public LoginResolutionPolicy? Policy { get; set; }

        public string Username { get; set; }
    }
}