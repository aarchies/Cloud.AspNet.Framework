using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.DotNet.Framework
{
    public interface IGlasssixBuilder
    {
        WebApplicationBuilder builder { get; }

        /// <summary>
        /// IConfiguration
        /// </summary>
        IConfiguration Configuration { get; set; }

        /// <summary>
        /// DI
        /// </summary>
        IServiceCollection Services { get; }

        IWebHostBuilder WebHostBuilder { get; set; }
    }
}