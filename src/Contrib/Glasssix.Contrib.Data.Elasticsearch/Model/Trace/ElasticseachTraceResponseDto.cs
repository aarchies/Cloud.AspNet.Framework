using System;
using System.Text.Json.Serialization;

namespace Glasssix.Contrib.Data.Elasticsearch.Model.Trace
{
    internal class ElasticseachTraceResponseDto : TraceResponseDto
    {
        [JsonPropertyName("EndTimestamp")]
        public override DateTime EndTimestamp { get; set; }

        [JsonPropertyName("@timestamp")]
        public override DateTime Timestamp { get; set; }
    }
}