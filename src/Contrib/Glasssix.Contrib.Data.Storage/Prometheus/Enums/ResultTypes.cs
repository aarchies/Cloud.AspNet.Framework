namespace Glasssix.Contrib.Data.Storage.Prometheus.Enums
{
    /// <summary>
    /// reference https://prometheus.io/docs/prometheus/latest/querying/api/#expression-query-result-formats
    /// </summary>
    public enum ResultTypes
    {
        Matrix = 1,
        Vector,
        Scalar,
        String
    }
}