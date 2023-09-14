using Glasssix.Contrib.Domain;

namespace Glasssix.Contrib.Repository.Base
{
    /// <summary>
    /// 查询仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IQueryRepository<TEntity> : IQueryRepository<TEntity, string> where TEntity : class, IAggregateRoot, IKey<string>
    {
    }

    /// <summary>
    /// 查询仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public interface IQueryRepository<TEntity, in TKey> : IQueryStore<TEntity, TKey> where TEntity : class, IAggregateRoot, IKey<TKey>
    {
    }
}