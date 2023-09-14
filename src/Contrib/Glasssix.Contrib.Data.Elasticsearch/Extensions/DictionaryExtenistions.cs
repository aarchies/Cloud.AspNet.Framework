using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Glasssix.Contrib.Data.Elasticsearch.Extensions
{
    /// <summary>
    /// 数据操作拓展类
    /// </summary>
    public static class DictionaryExtenistions
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// 根据Key类型分组描述符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="prefix"></param>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public static Dictionary<string, T> GroupByKeyPrefix<T>(this Dictionary<string, object> source, string prefix, Func<object, T>? convertFunc = null)
        {
            var result = new Dictionary<string, T>();
            foreach (var key in source.Keys)
            {
                if (!key.StartsWith(prefix))
                    continue;
                var value = source[key];
                var newKey = key[prefix.Length..];
                if (convertFunc != null)
                    value = convertFunc(source[key]);
                result.Add(newKey, (T)value!);
            }
            return result;
        }

        internal static T ConvertTo<T>(this Dictionary<string, object> dic)
        {
            var text = JsonSerializer.Serialize(dic, _serializerOptions);
            return JsonSerializer.Deserialize<T>(text, _serializerOptions)!;
        }
    }
}