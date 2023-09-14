using Glasssix.Contrib.Data.Serialization.Interfaces;
using Glasssix.Contrib.Data.TypeConverts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Glasssix.Contrib.Data.TypeConverts
{
    public class DefaultTypeConvertProvider : ITypeConvertProvider
    {
        private readonly IDeserializer? _deserializer;

        private readonly List<Type> _notNeedSerializeTypes = new List<Type>()
    {
        typeof(string),
        typeof(Guid),
        typeof(DateTime),
        typeof(decimal),
        typeof(Guid?),
        typeof(DateTime?),
        typeof(decimal?)
    };

        private readonly List<Type> _types = new List<Type>()
    {
        typeof(Guid),
        typeof(Guid?),
        typeof(DateTime),
        typeof(DateTime?)
    };

        public DefaultTypeConvertProvider(IDeserializer? deserializer = null) => _deserializer = deserializer;

        public T? ConvertTo<T>(string value, IDeserializer? deserializer = null)
        {
            var result = ConvertTo(value, typeof(T), deserializer);
            return result is T res ? res : default;
        }

        public object? ConvertTo(string value, Type type, IDeserializer? deserializer = null)
        {
            if (_types.Contains(type))
                return TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value)!;

            if (!IsSupportDeserialize(type))
                return System.Convert.ChangeType(value, type);

            return (deserializer ?? _deserializer)!.Deserialize(value, type);
        }

        private bool IsSupportDeserialize(Type type)
            => !type.IsPrimitive && !_notNeedSerializeTypes.Contains(type);
    }
}