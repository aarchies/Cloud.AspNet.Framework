using Glasssix.Contrib.Data.Elasticsearch.Model.LogModel;
using System;
using System.Text.Json.Serialization;

namespace Glasssix.Contrib.Data.Elasticsearch.Model
{
    internal class ElasticseachLogResponseDto : LogResponseDto
    {
        [JsonPropertyName("@timestamp")]
        public override DateTime Timestamp { get; set; }
    }
}