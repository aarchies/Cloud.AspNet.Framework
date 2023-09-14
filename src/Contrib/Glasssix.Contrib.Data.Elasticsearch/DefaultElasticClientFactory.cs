using Glasssix.Contrib.Data.Elasticsearch.Options;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public class DefaultElasticClientFactory : IElasticClientFactory
    {
        private readonly IOptionsMonitor<ElasticsearchFactoryOptions> _elasticsearchFactoryOptions;
        private readonly IServiceProvider _serviceProvider;

        public DefaultElasticClientFactory(IServiceProvider serviceProvider,
            IOptionsMonitor<ElasticsearchFactoryOptions> elasticsearchFactoryOptions)
        {
            _serviceProvider = serviceProvider;
            _elasticsearchFactoryOptions = elasticsearchFactoryOptions;
        }

        public IElasticClient Create()
        {
            var options = _elasticsearchFactoryOptions.CurrentValue;
            var defaultOptions = GetDefaultOptions(options.Options);
            if (defaultOptions == null)
                throw new NotSupportedException("No default ElasticClient found");

            return defaultOptions.Func.Invoke(_serviceProvider);
        }

        public IElasticClient Create(string name)
        {
            var options = _elasticsearchFactoryOptions.CurrentValue.Options.SingleOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (options == null)
                throw new NotSupportedException($"Please make sure you have used [{name}] ElasticClient, it was not found");

            return options.Func.Invoke(_serviceProvider);
        }

        private static ElasticsearchRelationsOptions? GetDefaultOptions(List<ElasticsearchRelationsOptions> optionsList)
        {
            return optionsList.SingleOrDefault(c => c.Name == Microsoft.Extensions.Options.Options.DefaultName) ??
                optionsList.FirstOrDefault();
        }
    }
}