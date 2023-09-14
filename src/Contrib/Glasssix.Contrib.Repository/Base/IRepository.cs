using Glasssix.Contrib.Domain;

namespace Glasssix.Contrib.Repository.Base
{
    /// <summary>
    /// 仓储(类型)
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public interface IRepository<TEntity, in TKey> : IQueryRepository<TEntity, TKey>, IStore<TEntity, TKey>
        where TEntity : class, IAggregateRoot, IKey<TKey>
    {
    }
}