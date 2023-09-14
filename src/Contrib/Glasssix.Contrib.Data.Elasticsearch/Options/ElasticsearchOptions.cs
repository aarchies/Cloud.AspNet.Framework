using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch.Options
{
    public class ElasticsearchOptions
    {
        private IEnumerable<string> _nodes;
        private bool? _useConnectionPool;

        public ElasticsearchOptions()
        {
            ConnectionSettingsOptions = new();
            StaticConnectionPoolOptions = new();
            Action = null;
        }

        public ElasticsearchOptions(params string[] nodes) : this() => Nodes = nodes;

        public Action<ConnectionSettings>? Action { get; set; }

        public ConnectionSettingsOptions ConnectionSettingsOptions { get; }

        public IEnumerable<string> Nodes
        {
            get => _nodes;
            set
            {
                if (value == null || !value.Any())
                    throw new ArgumentException("Please enter the Elasticsearch node address");

                _nodes = value;
            }
        }

        public StaticConnectionPoolOptions StaticConnectionPoolOptions { get; }

        public bool UseConnectionPool
        {
            get => _useConnectionPool == null && Nodes.Count() > 1 || _useConnectionPool == true;
            set => _useConnectionPool = value;
        }

        public ElasticsearchOptions UseConnectionSettings(Action<ConnectionSettings>? action)
        {
            Action = action;
            return this;
        }

        public ElasticsearchOptions UseDateTimeProvider(IDateTimeProvider? dateTimeProvider)
        {
            StaticConnectionPoolOptions.DateTimeProvider = dateTimeProvider;
            return this;
        }

        public ElasticsearchOptions UseNodes(params string[] nodes)
        {
            Nodes = nodes;
            return this;
        }

        public ElasticsearchOptions UseRandomize(bool randomize)
        {
            StaticConnectionPoolOptions.Randomize = randomize;
            return this;
        }
    }
}