namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Clients
{
    public class ClientSecretsApiDto
    {
        public ClientSecretsApiDto()
        {
            ClientSecrets = new List<ClientSecretApiDto>();
        }

        public List<ClientSecretApiDto> ClientSecrets { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}