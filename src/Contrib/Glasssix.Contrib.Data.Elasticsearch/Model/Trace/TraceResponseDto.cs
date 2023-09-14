using Glasssix.Contrib.Data.Elasticsearch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Glasssix.Contrib.Data.Elasticsearch.Model.Trace
{
    public class TraceResponseDto
    {
        private static readonly string[] databaseKeys = new string[] { "db.system" };
        private static readonly string[] exceptionKeys = new string[] { "exception.type", "exception.message" };
        private static readonly string[] httpKeys = new string[] { "http.method" };
        public virtual Dictionary<string, object> Attributes { get; set; }
        public virtual long Duration => (long)Math.Floor((EndTimestamp - Timestamp).TotalMilliseconds);
        public virtual DateTime EndTimestamp { get; set; }
        public virtual string Kind { get; set; }
        public virtual string Name { get; set; }
        public virtual string ParentSpanId { get; set; }
        public virtual Dictionary<string, object> Resource { get; set; }
        public virtual string SpanId { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual string TraceId { get; set; }
        public virtual int TraceStatus { get; set; }

        public virtual string GetDispalyName()
        {
            if (TryParseDatabase(out var databaseDto))
            {
                return $"{(Attributes.ContainsKey("peer.service") ? Attributes["peer.service"] : "")}{databaseDto.System}{databaseDto.Name}";
            }
            else if (TryParseHttp(out var traceHttpDto))
            {
                return traceHttpDto.Url;
            }
            else if (TryParseException(out TraceExceptionResponseDto exceptionDto))
            {
                return exceptionDto.Type ?? exceptionDto.Message;
            }
            else
                return Name;
        }

        public virtual bool TryParseDatabase(out TraceDatabaseResponseDto result)
        {
            result = default!;
            if (!IsContainsAnyKey(Attributes, databaseKeys))
                return false;
            result = Attributes.ConvertTo<TraceDatabaseResponseDto>();
            return true;
        }

        public virtual bool TryParseException(out TraceExceptionResponseDto result)
        {
            result = default!;
            if (!IsContainsAnyKey(Attributes, exceptionKeys))
                return false;

            result = Attributes.ConvertTo<TraceExceptionResponseDto>();
            return true;
        }

        public virtual bool TryParseHttp(out TraceHttpResponseDto result)
        {
            result = default!;
            if (!IsContainsAnyKey(Attributes, httpKeys))
                return false;
            result = Attributes.ConvertTo<TraceHttpResponseDto>();

            result.RequestHeaders = Attributes.GroupByKeyPrefix("http.request.header.", ReadHeaderValues);
            result.ReponseHeaders = Attributes.GroupByKeyPrefix("http.response.header.", ReadHeaderValues);

            result.Name = Name;
            result.Status = TraceStatus;
            return true;
        }

        private static bool IsContainsAnyKey(Dictionary<string, object> source, string[] keys)
        {
            if (source == null || !source.Any() || keys == null || !keys.Any())
                return false;
            if (keys.Length == 1)
                return source.ContainsKey(keys[0]);

            return keys.Any(k => source.ContainsKey(k));
        }

        private static IEnumerable<string> ReadHeaderValues(object obj)
        {
            if (obj is JsonElement value)
            {
                if (value.ValueKind == JsonValueKind.Array)
                {
                    return value.EnumerateArray().Select(item => item.ToString()).ToArray();
                }
                else
                {
                    return new string[] { value.ToString() };
                }
            }
            return new string[] { obj.ToString()! };
        }
    }
}