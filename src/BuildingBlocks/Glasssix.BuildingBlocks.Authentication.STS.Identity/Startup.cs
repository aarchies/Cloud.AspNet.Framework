using Glasssix.BuildingBlocks.Authentication.STS.Identity.Configuration;
using Glasssix.BuildingBlocks.Authentication.STS.Identity.Configuration.Constants;
using Glasssix.BuildingBlocks.Authentication.STS.Identity.Configuration.Interfaces;
using Glasssix.BuildingBlocks.Authentication.STS.Identity.Helpers;
using Glasssix.Contrib.Authentication.EntityFramework.Shared.DbContexts;
using Glasssix.Contrib.Authentication.EntityFramework.Shared.Entities.Identity;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Skoruba.IdentityServer4.Shared.Configuration.Helpers;

namespace Glasssix.BuildingBlocks.Authentication.STS.Identity
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UsePathBase(Configuration.GetValue<string>("BasePath"));

            app.UseStaticFiles();
            UseAuthentication(app);

            // Add custom security headers
            app.UseSecurityHeaders(Configuration);

            app.UseMvcLocalizationServices();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapDefaultControllerRoute();
                endpoint.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var rootConfiguration = CreateRootConfiguration();
            services.AddSingleton(rootConfiguration);
            // Register DbContexts for IdentityServer and Identity
            RegisterDbContexts(services);

            // Save data protection keys to db, using a common application name shared between Admin and STS
            services.AddDataProtection<IdentityServerDataProtectionDbContext>(Configuration);

            // Add email senders which is currently setup for SendGrid and SMTP
            services.AddEmailSenders(Configuration);

            // Add services for authentication, including Identity model and external providers
            RegisterAuthentication(services);

            // Add HSTS options
            RegisterHstsOptions(services);

            // Add all dependencies for Asp.Net Core Identity in MVC - these dependencies are injected into generic Controllers
            // Including settings for MVC and Localization
            // If you want to change primary keys or use another db model for Asp.Net Core Identity:
            services.AddMvcWithLocalization<UserIdentity, string>(Configuration);

            // Add authorization policies for MVC
            RegisterAuthorization(services);

            services.AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext, IdentityServerDataProtectionDbContext>(Configuration);
        }

        public virtual void RegisterAuthentication(IServiceCollection services)
        {
            services.AddAuthenticationServices<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(Configuration);
            services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity>(Configuration);
        }

        public virtual void RegisterAuthorization(IServiceCollection services)
        {
            var rootConfiguration = CreateRootConfiguration();
            services.AddAuthorizationPolicies(rootConfiguration);
        }

        public virtual void RegisterDbContexts(IServiceCollection services)
        {
            services.RegisterDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, IdentityServerDataProtectionDbContext>(Configuration);
        }

        public virtual void RegisterHstsOptions(IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }

        public virtual void UseAuthentication(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }

        protected IRootConfiguration CreateRootConfiguration()
        {
            var rootConfiguration = new RootConfiguration();
            Configuration.GetSection(ConfigurationConsts.AdminConfigurationKey).Bind(rootConfiguration.AdminConfiguration);
            Configuration.GetSection(ConfigurationConsts.RegisterConfigurationKey).Bind(rootConfiguration.RegisterConfiguration);
            return rootConfiguration;
        }
    }
}