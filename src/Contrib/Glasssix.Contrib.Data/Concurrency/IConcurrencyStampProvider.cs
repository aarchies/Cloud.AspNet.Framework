namespace Glasssix.Contrib.Data.Concurrency
{
    public interface IConcurrencyStampProvider
    {
        string GetRowVersion();
    }
}