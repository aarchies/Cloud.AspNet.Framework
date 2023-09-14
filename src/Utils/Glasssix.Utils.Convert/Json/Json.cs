using System.Text.Json;

namespace Glasssix.Utils.Convert.Json
{
    /// <summary>
    /// Json操作
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="isConvertToSingleQuotes">是否将双引号转成单引号</param>
        public static string ToJson(object target, bool isConvertToSingleQuotes = false)
        {
            if (target == null)
                return string.Empty;
            var result = JsonSerializer.Serialize(target, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            if (isConvertToSingleQuotes)
                result = result.Replace("\"", "'");
            return result;
        }

        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        public static T ToObject<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}