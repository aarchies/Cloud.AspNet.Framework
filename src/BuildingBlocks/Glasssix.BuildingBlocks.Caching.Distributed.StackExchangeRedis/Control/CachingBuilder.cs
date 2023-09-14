using Glasssix.Contrib.Caching.ClientFactory;
using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.BuildingBlocks.Caching.Distributed.StackExchangeRedis.Control
{
    public class CachingBuilder : ICachingBuilder
    {
        public CachingBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }
        public IServiceCollection Services { get; }
    }
}