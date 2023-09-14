using Glasssix.Contrib.Caching.Enumerations;
using System;

namespace Glasssix.Contrib.Caching.Helper
{
    /// <summary>
    /// Key订阅帮助类
    /// </summary>
    public static class SubscribeHelper
    {
        /// <summary>
        /// 格式化订阅Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string FormatSubscribeChannel<T>(string key, SubscribeKeyType type, string prefix = "")
        {
            var valueTypeFullName = typeof(T).FullName!;
            switch (type)
            {
                case SubscribeKeyType.ValueTypeFullName:
                    return valueTypeFullName;

                case SubscribeKeyType.ValueTypeFullNameAndKey:
                    return $"[{valueTypeFullName}]{key}";

                case SubscribeKeyType.SpecificPrefix:
                    return $"{prefix}{key}";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}