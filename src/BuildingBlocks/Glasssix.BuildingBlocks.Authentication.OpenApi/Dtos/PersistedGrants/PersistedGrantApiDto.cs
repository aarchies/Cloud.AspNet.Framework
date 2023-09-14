namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Dtos.PersistedGrants
{
    public class PersistedGrantApiDto
    {
        public string ClientId { get; set; }
        public DateTime? ConsumedTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }
        public DateTime? Expiration { get; set; }
        public string Key { get; set; }
        public string SessionId { get; set; }
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Type { get; set; }
    }
}