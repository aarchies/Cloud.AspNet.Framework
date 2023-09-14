using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Glasssix.Contrib.Data.Elasticsearch.Extenistions
{
    internal static class JsonElementExtensions
    {
        public static IEnumerable<KeyValuePair<string, object>> ConvertToKeyValuePairs(this JsonElement value)
        {
            if (value.ValueKind != JsonValueKind.Object)
                return default!;

            return GetObject(value);
        }

        public static object GetValue(this JsonElement value)
        {
            return value.ValueKind switch
            {
                JsonValueKind.Object => GetObject(value),
                JsonValueKind.Array => value.GetArray(),
                JsonValueKind.String => value.GetString()!,
                JsonValueKind.Number => GetNumber(value),
                JsonValueKind.True or JsonValueKind.False => value.GetBoolean(),
                _ => default!,
            };
        }

#pragma warning disable S6444

        private static object GetNumber(JsonElement value)
        {
            var str = value.GetRawText();

            if (Regex.IsMatch(str, @"\."))
            {
                return value.GetDouble();
            }
            else
            {
                if (!value.TryGetInt32(out int num))
                    return value.GetInt64();
                return num;
            }
        }

#pragma warning restore S6444

        private static IEnumerable<KeyValuePair<string, object>> GetObject(JsonElement value)
        {
            var result = new Dictionary<string, object>();
            foreach (var item in value.EnumerateObject())
            {
                var v = item.Value.GetValue();
                if (v == null)
                    continue;
                result.Add(item.Name, v);
            }
            if (result.Any())
                return result;
            return default!;
        }

        public static IEnumerable<object?> GetArray(this JsonElement value)
        {
            var temp = value.EnumerateArray();
            if (!temp.Any())
                return default!;
            var list = new List<object?>();
            foreach (var item in temp)
            {
                var v = item.GetValue();
                list.Add(v);
            }
            return list;
        }
    }
}