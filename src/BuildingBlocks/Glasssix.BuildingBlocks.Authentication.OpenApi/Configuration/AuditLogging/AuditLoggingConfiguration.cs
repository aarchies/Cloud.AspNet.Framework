namespace Glasssix.BuildingBlocks.Authentication.OpenApi.Configuration.AuditLogging
{
    public class AuditLoggingConfiguration
    {
        public string ClientIdClaim { get; set; }
        public string Source { get; set; }

        public string SubjectIdentifierClaim { get; set; }

        public string SubjectNameClaim { get; set; }
    }
}