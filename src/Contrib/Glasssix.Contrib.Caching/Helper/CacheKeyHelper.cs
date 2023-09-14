using Glasssix.Contrib.Caching.Enumerations;
using System;
using System.Collections.Generic;

namespace Glasssix.Contrib.Caching.Helper
{
    /// <summary>
    /// 缓存Key帮助类
    /// </summary>
    public static class CacheKeyHelper
    {
        /// <summary>
        /// 根据规则格式化Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheKeyType">缓存类型</param>
        /// <param name="typeAliasFunc"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string FormatCacheKey<T>(string key, CacheKeyType cacheKeyType, Func<string, string>? typeAliasFunc = null)
        {
            switch (cacheKeyType)
            {
                case CacheKeyType.None:
                    return key;

                case CacheKeyType.TypeName:
                    return $"{GetTypeName<T>()}.{key}";

                case CacheKeyType.TypeAlias:
                    if (typeAliasFunc == null)
                        throw new NotImplementedException();

                    var typeName = GetTypeName<T>();
                    return $"{typeAliasFunc.Invoke(typeName)}:{key}";

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTypeName<T>()
        {
            var type = typeof(T);
            if (type.IsGenericType)
            {
                var dictType = typeof(Dictionary<,>);
                if (type.GetGenericTypeDefinition() == dictType)
                    return type.Name + "[" + type.GetGenericArguments()[1].Name + "]";

                return type.Name + "[" + type.GetGenericArguments()[0].Name + "]";
            }

            return typeof(T).Name;
        }
    }
}