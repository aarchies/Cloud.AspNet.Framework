using System.Text.Json;

namespace Glasssix.Contrib.Data.Orm.SqlSugar
{
    public static class JsonExtensions
    {
        /// <summary>
        /// Json字符串反序列化成实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToEntity<T>(this string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(json, options)!;
        }
    }
}