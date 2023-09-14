using Glasssix.Contrib.Data.Options.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glasssix.Contrib.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectionStringNameAttribute : Attribute
    {
        private static readonly List<DbContextNameRelationOptions> DbContextNameRelationOptions = new List<DbContextNameRelationOptions>();

        public ConnectionStringNameAttribute(string name = "") => Name = name;

        public string Name { get; set; }

        public static string GetConnStringName<T>() => GetConnStringName(typeof(T));

        public static string GetConnStringName(Type type)
        {
            var options = DbContextNameRelationOptions.FirstOrDefault(c => c.DbContextType == type);
            if (options != null) return options.Name;

            var name = type.GetTypeInfo().GetCustomAttribute<ConnectionStringNameAttribute>()?.Name;
            if (string.IsNullOrWhiteSpace(name)) name = ConnectionStrings.DEFAULT_CONNECTION_STRING_NAME;
            DbContextNameRelationOptions.Add(new DbContextNameRelationOptions(name, type));
            return name!;
        }
    }
}