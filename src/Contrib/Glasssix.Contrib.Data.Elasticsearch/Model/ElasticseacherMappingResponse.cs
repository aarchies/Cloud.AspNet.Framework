namespace Glasssix.Contrib.Data.Elasticsearch.Model
{
    public class ElasticseacherMappingResponseDto : MappingResponseDto
    {
        /// <summary>
        /// if has keyword is true ,else false
        /// </summary>
        public bool? IsKeyword { get; set; }

        /// <summary>
        /// keyword query max length
        /// </summary>
        public int? MaxLenth { get; set; }
    }
}