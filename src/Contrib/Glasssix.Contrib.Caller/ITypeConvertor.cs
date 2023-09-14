using System.Collections.Generic;

namespace Glasssix.Contrib.Caller
{
    public interface ITypeConvertor
    {
        Dictionary<string, string> ConvertToDictionary<TRequest>(TRequest? request) where TRequest : class;

        IEnumerable<KeyValuePair<string, string>> ConvertToKeyValuePairs<TRequest>(TRequest? request) where TRequest : class;
    }
}