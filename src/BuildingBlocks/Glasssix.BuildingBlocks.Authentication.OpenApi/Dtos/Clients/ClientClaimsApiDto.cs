namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Clients
{
    public class ClientClaimsApiDto
    {
        public ClientClaimsApiDto()
        {
            ClientClaims = new List<ClientClaimApiDto>();
        }

        public List<ClientClaimApiDto> ClientClaims { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}