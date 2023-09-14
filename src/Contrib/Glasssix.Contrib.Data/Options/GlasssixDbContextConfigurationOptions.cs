namespace Glasssix.Contrib.Data.Options
{
    public class GlasssixDbContextConfigurationOptions
    {
        public GlasssixDbContextConfigurationOptions(string connectionString) => ConnectionString = connectionString;

        public string ConnectionString { get; }
    }
}