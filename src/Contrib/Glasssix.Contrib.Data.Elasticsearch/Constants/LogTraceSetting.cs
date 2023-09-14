using Glasssix.Contrib.Data.Elasticsearch.Model;
using System;

namespace Glasssix.Contrib.Data.Elasticsearch.Constants
{
    public class LogTraceSetting
    {
        internal LogTraceSetting(string indexName, bool isIndependent = false, string timestamp = "@timestamp")
        {
            if (!string.IsNullOrEmpty(IndexName) || string.IsNullOrEmpty(indexName))
                return;

            IndexName = indexName;
            Timestamp = timestamp;
            IsIndependent = isIndependent;
        }

        public string Timestamp { get; private set; }
        internal string IndexName { get; private set; }
        internal bool IsIndependent { get; private set; }

        internal Lazy<ElasticseacherMappingResponseDto[]> Mappings { get; set; }
    }
}