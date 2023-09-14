using Glasssix.BuildingBlocks.Authentication.STS.Identity.Helpers;
using Glasssix.Contrib.Authentication.EntityFramework.Shared.DbContexts;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity.Configuration.Test
{
    public class StartupTest : Startup
    {
        public StartupTest(IWebHostEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {
        }

        public override void RegisterDbContexts(IServiceCollection services)
        {
            services.RegisterDbContextsStaging<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, IdentityServerDataProtectionDbContext>();
        }
    }
}