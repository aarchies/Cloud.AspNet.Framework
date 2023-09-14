using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Glasssix.Contrib.Data
{
    [Serializable]
    public class ConnectionStrings : Dictionary<string, string>
    {
        public const string DEFAULT_CONNECTION_STRING_NAME = "DefaultConnection";
        public const string DEFAULT_SECTION = "ConnectionStrings";

        public ConnectionStrings()
        { }

        public ConnectionStrings(IEnumerable<KeyValuePair<string, string>> connectionStrings)
            : base(connectionStrings) { }

        protected ConnectionStrings(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string DefaultConnection
        {
            get => GetConnectionString(DEFAULT_CONNECTION_STRING_NAME);
            set => this[DEFAULT_CONNECTION_STRING_NAME] = value;
        }

        public string GetConnectionString(string name)
        {
            if (TryGetValue(name, out var connectionString))
                return connectionString;

            return string.Empty;
        }
    }
}