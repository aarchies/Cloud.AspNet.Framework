using System;

namespace Glasssix.Contrib.Data.Serialization.Interfaces
{
    public interface IDeserializer
    {
        TValue Deserialize<TValue>(string value);

        object? Deserialize(string value, Type valueType);
    }
}