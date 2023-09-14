using Glasssix.BuildingBlocks.Data.Uow;
using Glasssix.Contrib.Domain;

namespace Glasssix.Contrib.Repository.Base
{
    /// <summary>
    /// 仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体标识类型</typeparam>
    public class BaseRepository<TEntity, TKey> : StoreBase<TEntity, TKey>, IRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TEntity, TKey>
    {
        public BaseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}