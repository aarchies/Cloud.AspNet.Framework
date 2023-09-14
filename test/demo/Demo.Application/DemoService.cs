using Demo.Application.Contracts;
using Demo.Domain.Shared;
using Glasssix.BuildingBlocks.Data.Uow;
using Glasssix.Contrib.Repository.Base;
using Glasssix.Services;

namespace Demo.Application
{
    public class DemoService : SampleService<Domain.Model.Demo, DemoDto, DemoInput, long>, IDemoService
    {
        private readonly IRepository<Domain.Model.Demo, long> _repository;
        private readonly IUnitOfWork unitOfWork1;

        public DemoService(IUnitOfWork unitOfWork, IRepository<Domain.Model.Demo, long> repository) : base(unitOfWork, repository)
        {
            unitOfWork1 = unitOfWork;
            _repository = repository;
        }

        //public override async Task<string> CreateAsync(DemoDto request)
        //{
        //    using (var tr = await unitOfWork1.BeginTransactionAsync())
        //    {
        //        var model = new Demo.Domain.Model.Demo();
        //        model.PersonId = "323333";
        //        model.DeviceId = "333333";
        //        model.Init();
        //        await _repository.AddAsync(model);
        //        await unitOfWork1.CommitTransactionAsync(tr);
        //    }

        //    using (var tr = unitOfWork1.BeginTransactionScope())
        //    {
        //        var model = new Demo.Domain.Model.Demo();
        //        model.PersonId = "323333";
        //        model.DeviceId = "333333";
        //        model.Init();
        //        await _repository.AddAsync(model);
        //        await unitOfWork1.CommitTransactionAsync(tr);
        //    }

        //    return string.Empty;
        //}
    }
}