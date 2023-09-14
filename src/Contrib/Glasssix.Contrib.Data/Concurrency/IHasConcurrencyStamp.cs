namespace Glasssix.Contrib.Data.Concurrency
{
    public interface IHasConcurrencyStamp
    {
        string RowVersion { get; }
    }
}