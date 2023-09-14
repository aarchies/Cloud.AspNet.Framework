using Glasssix.BuildingBlocks.Data.Uow;
using Glasssix.BuildingBlocks.Repository.Base;

namespace Demo.Domain.Repositories.Implements
{
    public class DemoRepository : BaseRepository<Model.Demo, long>, IDemoRepository
    {
        public DemoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
