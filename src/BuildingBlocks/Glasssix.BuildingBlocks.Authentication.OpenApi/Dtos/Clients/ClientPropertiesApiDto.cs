namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Clients
{
    public class ClientPropertiesApiDto
    {
        public ClientPropertiesApiDto()
        {
            ClientProperties = new List<ClientPropertyApiDto>();
        }

        public List<ClientPropertyApiDto> ClientProperties { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}