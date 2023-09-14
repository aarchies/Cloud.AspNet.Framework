namespace Glasssix.Contrib.Data.Storage
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RequestPageBase
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}