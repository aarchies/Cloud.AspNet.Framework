using Demo.Domain.Shared;
using Glasssix.BuildingBlocks.DependencyInjection.Abstaractions;
using Glasssix.Contrib.Services.Abstractions;

namespace Demo.Application.Contracts
{
    public interface IDemoService : ISampleService<DemoDto, DemoInput>, IScopeDependency
    {
    }
}