using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Glasssix.Contrib.Data.Storage.Prometheus.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 当前支持的类型:类、结构和实现IEnumerable接口的类型,
        /// 结构和类默认使用public get属性和字段
        /// IEnumerable<KeyValuePair>类型直接转换为:key[]=value1&key[]=value2
        /// enum默认使用字符串。如果需要使用数值，请设置isEnumString=false
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isEnumString"></param>
        /// <param name="isCamelCase"></param>
        /// <param name="isUrlEncode"></param>
        /// <returns></returns>
        public static string? ToUrlParam(this object? obj, bool isEnumString = true, bool isCamelCase = true, bool isUrlEncode = true)
        {
            return GetValue(obj, string.Empty, isEnumString, isCamelCase, isUrlEncode);
        }

        private static string? AppendValue(string preStr, string? value, string splitChar, bool isUrlEncode)
        {
            if (string.IsNullOrEmpty(preStr) || string.IsNullOrEmpty(value))
                return value;

            if (isUrlEncode)
                return $"{preStr}{splitChar}{HttpUtility.UrlEncode(value, Encoding.UTF8)}";
            else
                return $"{preStr}{splitChar}{value}";
        }

        private static string? GetEnumerableValue(object obj, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
        {
            var list = new List<string>();
            foreach (var item in (IEnumerable)obj)
            {
                if (item is KeyValuePair<string, object> keyValue)
                {
                    var name = keyValue.Key;
                    if (isCamelCase)
                        name = name.ToCamelCase();
                    var str = GetValue(keyValue.Value, AppendValue(preStr, name, ".", isUrlEncode) ?? default!, isEnumString, isCamelCase, isUrlEncode);
                    if (!string.IsNullOrEmpty(str))
                        list.Add(str);
                }
                else
                {
                    var str = GetValue(item, $"{preStr}{(isUrlEncode ? HttpUtility.UrlEncode("[]", Encoding.UTF8) : "[]")}", isEnumString, isCamelCase, isUrlEncode);
                    if (!string.IsNullOrEmpty(str))
                        list.Add(str);
                }
            }
            if (!list.Any())
                return default!;

            list.Sort();
            return string.Join('&', list);
        }

        private static string? GetMemberInfoValue(MemberInfo info, object? value, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
        {
            if (value == null)
                return null;

            var name = info.Name;
            if (isCamelCase)
                name = name.ToCamelCase();

            return GetValue(value, AppendValue(preStr, name, ".", isUrlEncode) ?? default!, isEnumString, isCamelCase, isUrlEncode);
        }

        private static string GetObjValue(Type type, object obj, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
            var list = new List<string>();

            foreach (var item in properties)
            {
                var str = GetMemberInfoValue(item, item.GetValue(obj), preStr, isEnumString, isCamelCase, isUrlEncode);
                if (string.IsNullOrEmpty(str))
                    continue;
                list.Add(str);
            }

            foreach (var item in fields)
            {
                var str = GetMemberInfoValue(item, item.GetValue(obj), preStr, isEnumString, isCamelCase, isUrlEncode);
                if (string.IsNullOrEmpty(str))
                    continue;
                list.Add(str);
            }

            if (!list.Any())
                return default!;

            list.Sort();
            return string.Join('&', list);
        }

        private static string? GetValue(object? obj, string preStr, bool isEnumString = false, bool isCamelCase = true, bool isUrlEncode = true)
        {
            if (obj == null) return null;
            var type = obj.GetType();
            if (type == typeof(string))
            {
                var str = (string)obj;
                return AppendValue(preStr, str, "=", isUrlEncode);
            }

            if (type.IsValueType)
            {
                if (type.IsEnum)
                {
                    var str = isEnumString ? obj.ToString() : Convert.ToInt32(obj).ToString();
                    return AppendValue(preStr, str, "=", isUrlEncode);
                }

                //sample value
                if (type.IsPrimitive)
                {
                    var str = obj.ToString();
                    return AppendValue(preStr, str, "=", isUrlEncode);
                }

                //struct
                return GetObjValue(type, obj, preStr, isEnumString, isCamelCase, isUrlEncode);
            }

            if (type.IsArray || type.GetInterfaces().Any(t => t.Name.IndexOf("IEnumerable") == 0))
                return GetEnumerableValue(obj, preStr, isEnumString, isCamelCase, isUrlEncode);

            if (type.IsClass)
                return GetObjValue(type, obj, preStr, isEnumString, isCamelCase, isUrlEncode);

            //当前类型不支持
            return null;
        }
    }
}