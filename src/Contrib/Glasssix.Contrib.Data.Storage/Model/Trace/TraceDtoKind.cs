namespace Glasssix.Contrib.Data.Storage.Model.Trace
{
    public sealed class TraceDtoKind
    {
        public const string SPAN_KIND_CLIENT = nameof(SPAN_KIND_CLIENT);

        public const string SPAN_KIND_SERVER = nameof(SPAN_KIND_SERVER);

        private TraceDtoKind()
        { }
    }
}