using Glasssix.Contrib.Data.Serialization.Interfaces;
using System;

namespace Glasssix.Contrib.Data.TypeConverts.Interfaces
{
    public interface ITypeConvertProvider
    {
        T? ConvertTo<T>(string value, IDeserializer? deserializer = null);

        object? ConvertTo(string value, Type type, IDeserializer? deserializer = null);
    }
}