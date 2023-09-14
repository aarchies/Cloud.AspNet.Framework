namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.Clients
{
    public class ClientsApiDto
    {
        public ClientsApiDto()
        {
            Clients = new List<ClientApiDto>();
        }

        public List<ClientApiDto> Clients { get; set; }

        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}