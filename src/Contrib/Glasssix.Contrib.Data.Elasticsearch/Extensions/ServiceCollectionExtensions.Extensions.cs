using Glasssix.Contrib.Data.Elasticsearch;
using Glasssix.Contrib.Data.Elasticsearch.Options;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static GlasssixElasticsearchBuilder AddElasticsearchClient(this IServiceCollection services)
            => services.AddElasticsearch(Constant.DEFAULT_CLIENT_NAME).CreateElasticsearchClient(Constant.DEFAULT_CLIENT_NAME);

        public static GlasssixElasticsearchBuilder AddElasticsearchClient(this IServiceCollection services, IEnumerable<string> nodes)
            => services.AddElasticsearch(Constant.DEFAULT_CLIENT_NAME, nodes).CreateElasticsearchClient(Constant.DEFAULT_CLIENT_NAME);

        public static GlasssixElasticsearchBuilder AddElasticsearchClient(this IServiceCollection services, Action<ElasticsearchOptions> action)
            => services.AddElasticsearchClient(Constant.DEFAULT_CLIENT_NAME, action);

        public static GlasssixElasticsearchBuilder AddElasticsearchClient(this IServiceCollection services, string name, params string[] nodes)
            => services.AddElasticsearch(name, nodes).CreateElasticsearchClient(name);

        public static GlasssixElasticsearchBuilder AddElasticsearchClient(this IServiceCollection services, string name, IEnumerable<string> nodes)
            => services.AddElasticsearch(name, nodes).CreateElasticsearchClient(name);

        public static GlasssixElasticsearchBuilder AddElasticsearchClient(this IServiceCollection services,
            string name,
            Action<ElasticsearchOptions> action)
            => services.AddElasticsearch(name, action).CreateElasticsearchClient(name);

        private static GlasssixElasticsearchBuilder CreateElasticsearchClient(this IServiceCollection services, string name)
                                    => new(services, name);
    }
}