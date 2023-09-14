using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data.Orm.Dapper.Extensions.String
{
    public enum BaseTypeMode
    {
        Interface = 1,
        Abstract = 2,
        Class = 3
    }

    public static class TypeExtensions
    {
        /// <summary>
        /// 获取所有实现自此的类
        /// </summary>
        /// <typeparam name="T">BaseType</typeparam>
        /// <param name="baseTypeMode">BaseType 类型</param>
        /// <returns></returns>
        public static Type[] GetImplementsType<T>(BaseTypeMode baseTypeMode)
        {
            //var assemblyList = AssemblyExtensions.GetAllAssemblies().SelectMany(t => t.GetTypes()).ToList();
            //if (assemblyList.Any())
            var assemblyList = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).SelectMany(x => x.ExportedTypes).ToList();
            var typeQuery = assemblyList
                .Where(t => typeof(T).IsAssignableFrom(t) || t.BaseType == typeof(T));
            return baseTypeMode switch
            {
                BaseTypeMode.Interface => typeQuery.Where(t => t.IsInterface && !t.IsAbstract && !t.IsClass).ToArray(),
                BaseTypeMode.Abstract => typeQuery.Where(t => !t.IsInterface && t.IsAbstract && !t.IsClass).ToArray(),
                BaseTypeMode.Class => typeQuery.Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass).ToArray(),
                _ => throw new ArgumentOutOfRangeException(nameof(baseTypeMode), baseTypeMode, null)
            };
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 转换为用分隔符连接的字符串
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string Join<T>(this IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            return StringExtensions.Join<T>(list, quotes, separator);
        }

        /// <summary>
        /// 安全获取值，当值为null时，不会抛出异常
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default;
        }
    }
}