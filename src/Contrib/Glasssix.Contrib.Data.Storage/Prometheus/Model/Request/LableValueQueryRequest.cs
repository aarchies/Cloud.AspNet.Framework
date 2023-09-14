namespace Glasssix.Contrib.Data.Storage.Prometheus.Model.Request
{
    public class LableValueQueryRequest : MetaDataQueryRequest
    {
        public string Lable { get; set; } = "__name__";
    }
}