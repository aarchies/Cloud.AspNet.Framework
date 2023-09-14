namespace Glasssix.Contrib.Data.Elasticsearch.Constants
{
    public static class ElasticConstant
    {
        internal const string DEFAULT_CALLER_CLIENT_NAME = "Glasssix.contrib.stacksdks.tsc.log.elasticseach.all";
        internal const string LOG_CALLER_CLIENT_NAME = "Glasssix.contrib.stacksdks.tsc.log.elasticseach.log";
        internal const string TRACE_CALLER_CLIENT_NAME = "Glasssix.contrib.stacksdks.tsc.log.elasticseach.trace";
        private const string TIMESTAMP = "@timestamp";
        public static string Endpoint => "Attributes.http.url";
        public static LogTraceSetting Log { get; private set; }
        public static string NameSpace => "Resource.service.namespace";
        public static string ParentId => "ParentSpanId";
        public static string ServiceInstance => "Resource.service.instance.id";
        public static string ServiceName => "Resource.service.name";
        public static string SpanId => "SpanId";
        public static LogTraceSetting Trace { get; private set; }
        public static string TraceId => "TraceId";
        internal static int MaxRecordCount { get; private set; } = 10000;

        internal static void InitLog(string indexName, bool isIndependent = false)
        {
            if (Log != null || string.IsNullOrEmpty(indexName))
                return;
            Log = new LogTraceSetting(indexName, isIndependent, TIMESTAMP);
        }

        internal static void InitTrace(string indexName, bool isIndependent = false)
        {
            if (Trace != null || string.IsNullOrEmpty(indexName))
                return;
            Trace = new LogTraceSetting(indexName, isIndependent, TIMESTAMP);
        }
    }
}