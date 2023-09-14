using Elasticsearch.Net;
using Nest;

namespace Glasssix.Contrib.Data.Elasticsearch.Options
{
    public class ConnectionSettingsOptions
    {
        public ConnectionSettingsOptions()
        {
            Connection = null;
            SourceSerializerFactory = null;
            PropertyMappingProvider = null;
        }

        public IConnection? Connection { get; set; }

        public IPropertyMappingProvider? PropertyMappingProvider { get; set; }
        public ConnectionSettings.SourceSerializerFactory? SourceSerializerFactory { get; set; }

        public ConnectionSettingsOptions UseConnection(IConnection? connection)
        {
            Connection = connection;
            return this;
        }

        public ConnectionSettingsOptions UsePropertyMappingProvider(IPropertyMappingProvider? propertyMappingProvider)
        {
            PropertyMappingProvider = propertyMappingProvider;
            return this;
        }

        public ConnectionSettingsOptions UseSourceSerializerFactory(ConnectionSettings.SourceSerializerFactory? sourceSerializerFactory)
        {
            SourceSerializerFactory = sourceSerializerFactory;
            return this;
        }
    }
}