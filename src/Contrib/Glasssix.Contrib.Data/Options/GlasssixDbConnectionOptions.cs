using System;
using System.Linq;

namespace Glasssix.Contrib.Data.Options
{
    public class GlasssixDbConnectionOptions
    {
        public GlasssixDbConnectionOptions()
        {
            ConnectionStrings = new ConnectionStrings();
        }

        public ConnectionStrings ConnectionStrings { get; set; }

        public void TryAddConnectionString(string name, string connectionString)
        {
            if (ConnectionStrings.All(item => !item.Key.Equals(name, StringComparison.OrdinalIgnoreCase)))
                ConnectionStrings.Add(name, connectionString);
        }
    }
}