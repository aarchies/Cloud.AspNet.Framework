using Glasssix.Contrib.Data.Elasticsearch.Extenistions;
using Glasssix.Contrib.Data.Elasticsearch.Extensions;
using Glasssix.Contrib.Data.Elasticsearch.Model.Trace;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Glasssix.Contrib.Data.Elasticsearch.Converters
{
    internal class TraceResponseDtoConverter : JsonConverter<TraceResponseDto>
    {
        public override TraceResponseDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonDocument.TryParseValue(ref reader, out var doc))
            {
                var jsonObject = doc.RootElement;
                var rootText = jsonObject.GetRawText();
                var result = JsonSerializer.Deserialize<ElasticseachTraceResponseDto>(rootText);
                if (result == null)
                    return default;
                if (result.Timestamp == DateTime.MinValue || result.Timestamp == DateTime.MaxValue)
                    return default;
                if (string.IsNullOrEmpty(result.TraceId))
                    return default;

                result.Attributes = jsonObject.ConvertToKeyValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value).GroupByKeyPrefix<object>("Attributes.");
                result.Resource = jsonObject.ConvertToKeyValuePairs().ToDictionary(kv => kv.Key, kv => kv.Value).GroupByKeyPrefix<object>("Resource.");

                return result;
            }

            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TraceResponseDto value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}