using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.ApiResources
{
    public class ApiSecretApiDto
    {
        public string Description { get; set; }

        public DateTime? Expiration { get; set; }

        public int Id { get; set; }

        [Required]
        public string Type { get; set; } = "SharedSecret";

        [Required]
        public string Value { get; set; }
    }
}