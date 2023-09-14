using System.Reflection;
using System.Text.Json;

namespace Glasssix.Contrib.Caller.Internal.Options
{
    internal class PropertyInfoMember
    {
        public PropertyInfoMember(PropertyInfo property, string name, bool needSerialize)
        {
            Property = property;
            Name = name;
            NeedSerialize = needSerialize;
        }

        public string Name { get; }
        public bool NeedSerialize { get; }
        public PropertyInfo Property { get; }

        public bool TryGetValue<TRequest>(TRequest data, out string value) where TRequest : class
        {
            value = string.Empty;
            var propertyValue = Property.GetValue(data);
            if (propertyValue == null || !NeedSerialize && propertyValue.ToString() == null)
                return false;

            value = !NeedSerialize ? propertyValue.ToString()! : JsonSerializer.Serialize(propertyValue);
            return true;
        }
    }
}