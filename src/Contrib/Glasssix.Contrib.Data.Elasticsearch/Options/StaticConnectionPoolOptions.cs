using Elasticsearch.Net;

namespace Glasssix.Contrib.Data.Elasticsearch.Options
{
    public class StaticConnectionPoolOptions
    {
        public StaticConnectionPoolOptions()
        {
            Randomize = true;
            DateTimeProvider = null;
        }

        public IDateTimeProvider? DateTimeProvider { get; set; }
        public bool Randomize { get; set; }
    }
}