using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.Contrib.Caller
{
    public class CallerOptionsBuilder
    {
        public CallerOptionsBuilder(IServiceCollection services, string name)
        {
            Services = services;
            Name = name;
        }

        public string Name { get; }
        public IServiceCollection Services { get; }
    }
}