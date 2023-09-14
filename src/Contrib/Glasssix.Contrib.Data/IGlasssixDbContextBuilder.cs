using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.Contrib.Data
{
    public interface IGlasssixDbContextBuilder
    {
        public bool EnableSoftDelete { get; set; }

        public IServiceCollection Services { get; }
    }
}