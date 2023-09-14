namespace Glasssix.BuildingBlocks.Authentication.AdminConsole.Configuration.Test
{
    public class StartupTest : Startup
    {
        public StartupTest(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
        }

        public override void ConfigureUIOptions(IdentityServer4AdminUIOptions options)
        {
            base.ConfigureUIOptions(options);

            // Use staging DbContexts and auth services.
            options.Testing.IsStaging = true;
        }
    }
}