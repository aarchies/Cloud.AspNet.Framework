using Glasssix.Utils.Convert.Objects;
using Glasssix.Utils.Convert.Strings;
using Glasssix.Utils.Convert.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Glasssix.Utils.Convert.Strings
{
    public static class StringExtensions
    {
        public static StringBuilder Append(this StringBuilder builder, Exception exception, int maxDepth)
        {
            return builder.Append(exception, 0, maxDepth);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 判断字符串是否不为空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotNullOrWhitespace(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 移除末尾字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="removeValue">要移除的值</param>
        public static string RemoveEnd(this string value, string removeValue)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(removeValue))
                return value;
            if (value.ToLower().EndsWith(removeValue.ToLower()))
                return value.Remove(value.Length - removeValue.Length, removeValue.Length);
            return value;
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        public static string Sha256(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            var managed = new SHA256Managed();
            return managed.ComputeHash(bytes).ToHexString();
        }

        /// <summary>
        /// 分割逗号的字符串为List
        /// </summary>
        /// <param name="csvList"></param>
        /// <param name="nullOrWhitespaceInputReturnsNull">nullorwhitespace字符串是否返回空对象</param>
        /// <returns></returns>
        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable()
                .Select(s => s.Trim())
                .ToList();
        }

        /// <summary>
        /// 通用泛型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="input">输入值</param>
        public static T To<T>(this object input)
        {
            if (input == null)
                return default;
            if (input is string && string.IsNullOrWhiteSpace(input.ToString()))
                return default;
            Type type = TypeExtensions.GetType<T>();
            var typeName = type.Name.ToLower();
            try
            {
                if (typeName == "string")
                    return (T)(object)input.ToString();
                if (typeName == "guid")
                    return (T)(object)new Guid(input.ToString());
                if (type.IsEnum)
                    return Enum.Enum.Parse<T>(input);
                if (input is IConvertible)
                    return (T)System.Convert.ChangeType(input, type);
                return (T)input;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 字符转ASCII码
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static int ToASCII(this string character)
        {
            if (character.Length != 1)
                throw new Exception("无效字符");
            var asciiEncoding = new ASCIIEncoding();
            int intAsciiCode = asciiEncoding.GetBytes(character)[0];
            return intAsciiCode;
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid ToGuid(this string obj)
        {
            return Guid.Parse(obj);
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="input">输入值</param>
        public static Guid ToGuid(object input)
        {
            return ToGuidOrNull(input) ?? Guid.Empty;
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="input">输入值</param>
        public static Guid? ToGuidOrNull(object input)
        {
            Guid.TryParse(input.SafeString(), out var result);
            if (result == null)
                return null;
            return result;
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid? ToGuidOrNull(this string obj)
        {
            return ObjectExtensions.ToGuidOrNull(obj);
        }

        /// <summary>
        /// Hex
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] text)
        {
            StringBuilder ret = new StringBuilder();
            foreach (byte b in text)
            {
                ret.AppendFormat("{0:x2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt(this string obj)
        {
            return ObjectExtensions.ToInt(obj);
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int? ToIntOrNull(this string obj)
        {
            return ObjectExtensions.ToIntOrNull(obj);
        }

        /// <summary>
        /// 泛型集合转换
        /// </summary>
        /// <typeparam name="T">目标元素类型</typeparam>
        /// <param name="input">以逗号分隔的元素集合字符串，范例:83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A</param>
        public static List<T> ToList<T>(this string input)
        {
            var result = new List<T>();
            if (string.IsNullOrWhiteSpace(input))
                return result;
            var array = input.Split(',');
            result.AddRange(from each in array where !string.IsNullOrWhiteSpace(each) select each.To<T>());
            return result;
        }

        /// <summary>
        /// 转换为long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong(this string obj)
        {
            return ObjectExtensions.ToLong(obj);
        }

        /// <summary>
        /// 转换为可空long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long? ToLongOrNull(this string obj)
        {
            return ObjectExtensions.ToLongOrNull(obj);
        }

        /// <summary>
        /// 过滤开头字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="trim"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static string TrimStart(this string source, string trim, StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (source == null)
            {
                return null;
            }
            string s = source;
            while (s.StartsWith(trim, stringComparison))
            {
                s = s.Substring(trim.Length);
            }
            return s;
        }

        #region Join(将集合连接为带分隔符的字符串)

        /// <summary>
        /// 将集合连接为带分隔符的字符串
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string Join<T>(IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            if (list == null)
                return string.Empty;
            var result = new StringBuilder();
            foreach (var each in list)
                result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
            if (separator == "")
                return result.ToString();
            return result.ToString().TrimEnd(separator.ToCharArray());
        }

        #endregion Join(将集合连接为带分隔符的字符串)

        #region SplitWordGroup(分隔词组)

        /// <summary>
        /// 分隔词组
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="separator">分隔符，默认使用"-"分隔</param>
        public static string SplitWordGroup(this string value, char separator = '-')
        {
            var pattern = @"([A-Z])(?=[a-z])|(?<=[a-z])([A-Z]|[0-9]+)";
            return string.IsNullOrWhiteSpace(value) ? string.Empty : System.Text.RegularExpressions.Regex.Replace(value, pattern, $"{separator}$1$2").TrimStart(separator).ToLower();
        }

        #endregion SplitWordGroup(分隔词组)

        #region FirstLowerCase(首字母小写)

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="value">值</param>
        public static string FirstLowerCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            return $"{value.Substring(0, 1).ToLower()}{value.Substring(1)}";
        }

        #endregion FirstLowerCase(首字母小写)

        #region FirstUpperCase(首字母大写)

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="value">值</param>
        public static string FirstUpperCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            return $"{value.Substring(0, 1).ToUpper()}{value.Substring(1)}";
        }

        #endregion FirstUpperCase(首字母大写)

        #region PinYin(获取汉字的拼音简码)

        /// <summary>
        /// 使用字符编码方式获取拼音简码
        /// </summary>
        private static string ResolveByCode(ushort unicode)
        {
            if (unicode >= '\uB0A1' && unicode <= '\uB0C4')
                return "A";
            if (unicode >= '\uB0C5' && unicode <= '\uB2C0' && unicode != 45464)
                return "B";
            if (unicode >= '\uB2C1' && unicode <= '\uB4ED')
                return "C";
            if (unicode >= '\uB4EE' && unicode <= '\uB6E9')
                return "D";
            if (unicode >= '\uB6EA' && unicode <= '\uB7A1')
                return "E";
            if (unicode >= '\uB7A2' && unicode <= '\uB8C0')
                return "F";
            if (unicode >= '\uB8C1' && unicode <= '\uB9FD')
                return "G";
            if (unicode >= '\uB9FE' && unicode <= '\uBBF6')
                return "H";
            if (unicode >= '\uBBF7' && unicode <= '\uBFA5')
                return "J";
            if (unicode >= '\uBFA6' && unicode <= '\uC0AB')
                return "K";
            if (unicode >= '\uC0AC' && unicode <= '\uC2E7')
                return "L";
            if (unicode >= '\uC2E8' && unicode <= '\uC4C2')
                return "M";
            if (unicode >= '\uC4C3' && unicode <= '\uC5B5')
                return "N";
            if (unicode >= '\uC5B6' && unicode <= '\uC5BD')
                return "O";
            if (unicode >= '\uC5BE' && unicode <= '\uC6D9')
                return "P";
            if (unicode >= '\uC6DA' && unicode <= '\uC8BA')
                return "Q";
            if (unicode >= '\uC8BB' && unicode <= '\uC8F5')
                return "R";
            if (unicode >= '\uC8F6' && unicode <= '\uCBF9')
                return "S";
            if (unicode >= '\uCBFA' && unicode <= '\uCDD9')
                return "T";
            if (unicode >= '\uCDDA' && unicode <= '\uCEF3')
                return "W";
            if (unicode >= '\uCEF4' && unicode <= '\uD188')
                return "X";
            if (unicode >= '\uD1B9' && unicode <= '\uD4D0')
                return "Y";
            if (unicode >= '\uD4D1' && unicode <= '\uD7F9')
                return "Z";
            return string.Empty;
        }

        #endregion PinYin(获取汉字的拼音简码)

        private static StringBuilder Append(this StringBuilder builder, Exception exception, int currentDepth, int maxDepth)
        {
            try
            {
                builder.Append(exception.GetType());
                if (!string.IsNullOrEmpty(exception.Message))
                {
                    builder.Append(": ").Append(exception.Message);
                }

                if (!string.IsNullOrEmpty(exception.StackTrace))
                {
                    builder.AppendLine();
                    builder.Append(exception.StackTrace);
                }

                if (++currentDepth >= maxDepth && maxDepth > 0)
                    return builder;

                if (exception is AggregateException aggEx)
                {
                    foreach (var ex in aggEx.InnerExceptions)
                    {
                        builder.AppendInnerException(ex, currentDepth, maxDepth);
                        if (++currentDepth >= maxDepth && maxDepth > 0)
                            return builder;
                    }
                }
                else if (exception.InnerException != null)
                {
                    builder.AppendInnerException(exception.InnerException, currentDepth, maxDepth);
                }
            }
            catch
            {
                // Processing exceptions shouldn't throw exceptions
            }

            return builder;
        }

        private static void AppendInnerException(this StringBuilder builder, Exception exception, int currentDepth, int maxDepth)
            => builder.AppendLine()
                .Append(" ---> ")
                .Append(exception, currentDepth, maxDepth)
                .AppendLine()
                .Append("   --- End of inner exception stack trace ---");
    }
}