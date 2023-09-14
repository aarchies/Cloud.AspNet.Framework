using Skoruba.IdentityServer4.Admin.EntityFramework.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Clients
{
    public class ClientSecretApiDto
    {
        public string Description { get; set; }

        public DateTime? Expiration { get; set; }

        public string HashType { get; set; }

        public HashType HashTypeEnum => Enum.TryParse(HashType, true, out HashType result) ? result : Skoruba.IdentityServer4.Admin.EntityFramework.Helpers.HashType.Sha256;

        public int Id { get; set; }

        [Required]
        public string Type { get; set; } = "SharedSecret";

        [Required]
        public string Value { get; set; }
    }
}