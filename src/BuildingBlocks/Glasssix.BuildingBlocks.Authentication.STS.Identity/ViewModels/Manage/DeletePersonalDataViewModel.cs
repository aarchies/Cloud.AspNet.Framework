using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.ViewModels.Manage
{
    public class DeletePersonalDataViewModel
    {
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public bool RequirePassword { get; set; }
    }
}