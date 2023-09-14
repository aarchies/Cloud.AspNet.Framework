using Demo.Application.Contracts;
using Demo.Domain.Shared;
using Glasssix.BuildingBlocks.Data.Uow;
using Glasssix.BuildingBlocks.Repository.Base;
using Glasssix.Services;

namespace Demo.Application
{
    public class DemoService : SampleService<Domain.Model.Demo, DemoDto, DemoInput, long>, IDemoService
    {
        public DemoService(IUnitOfWork unitOfWork, IRepository<Domain.Model.Demo, long> repository) : base(unitOfWork, repository)
        {

        }

    }
}