using System.Text.Json.Serialization;

namespace Glasssix.Contrib.Data.Elasticsearch.Model.Trace
{
    public class TraceExceptionResponseDto
    {
        [JsonPropertyName("exception.escaped")]
        public virtual bool Escaped { get; set; }

        [JsonPropertyName("exception.message")]
        public virtual string Message { get; set; }

        [JsonPropertyName("exception.stacktrace")]
        public virtual string StackTrace { get; set; }

        [JsonPropertyName("exception.type")]
        public virtual string Type { get; set; }
    }
}