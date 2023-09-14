using IdentityAuthServer.STS.Identity.Configuration.Interfaces;
using Skoruba.IdentityServer4.Shared.Configuration.Configuration.Identity;

namespace IdentityAuthServer.STS.Identity.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {
        public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
        public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
    }
}







