namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.ApiResources
{
    public class ApiSecretsApiDto
    {
        public ApiSecretsApiDto()
        {
            ApiSecrets = new List<ApiSecretApiDto>();
        }

        public List<ApiSecretApiDto> ApiSecrets { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}