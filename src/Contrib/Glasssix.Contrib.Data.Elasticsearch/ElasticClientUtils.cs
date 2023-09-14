using Elasticsearch.Net;
using Glasssix.Contrib.Data.Elasticsearch.Options;
using Nest;
using System;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public static class ElasticClientUtils
    {
        public static IElasticClient Create(ElasticsearchOptions elasticsearchOptions)
        {
            var settings = elasticsearchOptions.UseConnectionPool
                ? GetConnectionSettingsConnectionPool(elasticsearchOptions)
                : GetConnectionSettingsBySingleNode(elasticsearchOptions);
            return new ElasticClient(settings);
        }

        private static ConnectionSettings GetConnectionSettingsBySingleNode(ElasticsearchOptions relation)
        {

            var connectionSetting = new ConnectionSettings(new Uri(relation.Nodes.First()))
                .EnableApiVersioningHeader(false);
            relation.Action?.Invoke(connectionSetting);

            return connectionSetting;
        }

        private static ConnectionSettings GetConnectionSettingsConnectionPool(ElasticsearchOptions relation)
        {
            var pool = new StaticConnectionPool(
                relation.Nodes.Select(node => new Uri(node)),
                relation.StaticConnectionPoolOptions.Randomize,
                relation.StaticConnectionPoolOptions.DateTimeProvider);

            var settings = new ConnectionSettings(
                    pool,
                    relation.ConnectionSettingsOptions.Connection,
                    relation.ConnectionSettingsOptions.SourceSerializerFactory,
                    relation.ConnectionSettingsOptions.PropertyMappingProvider)
                .EnableApiVersioningHeader(false);

            relation.Action?.Invoke(settings);
            return settings;
        }
    }
}