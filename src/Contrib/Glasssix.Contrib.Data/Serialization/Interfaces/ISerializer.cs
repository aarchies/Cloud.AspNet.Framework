namespace Glasssix.Contrib.Data.Serialization.Interfaces
{
    public interface ISerializer
    {
        string Serialize<TValue>(TValue value);
    }
}