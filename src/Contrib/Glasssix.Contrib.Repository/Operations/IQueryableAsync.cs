using Glasssix.Contrib.Domain;

namespace Glasssix.Contrib.Repository.Operations
{
    public interface IQueryableAsync<TEntity, in TKey> where TEntity : class, IKey<TKey>
    {
    }
}