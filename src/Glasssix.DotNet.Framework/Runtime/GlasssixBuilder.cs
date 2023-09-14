using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glasssix.DotNet.Framework.Runtime
{
    public class GlasssixBuilder : IGlasssixBuilder
    {
        public GlasssixBuilder(WebApplicationBuilder services)
        {
            builder = services;
            WebHostBuilder = services.WebHost;
            Configuration = services.Configuration;
            Services = services.Services;
        }

        public WebApplicationBuilder builder { get; set; }
        public IConfiguration Configuration { get; set; }
        public IServiceCollection Services { get; private set; }
        public IWebHostBuilder WebHostBuilder { get; set; }
    }
}