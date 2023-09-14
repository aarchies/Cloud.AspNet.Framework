namespace Glasssix.Contrib.Data.Elasticsearch.Constants
{
    internal static class MappingConstant
    {
        public const string FIELD = "fields";
        public const string KEYWORD = "keyword";

        /// <summary>
        /// when field is keyword, the maximum field value length, fields beyond this length will not be indexed, but will be stored
        /// </summary>
        public const string MAXLENGTH = "ignore_above";

        public const string PROPERTY = "properties";
        public const string TYPE = "type";
    }
}