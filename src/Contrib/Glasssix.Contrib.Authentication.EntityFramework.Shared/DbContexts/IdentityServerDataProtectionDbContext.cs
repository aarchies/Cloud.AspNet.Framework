using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Glasssix.Contrib.Authentication.EntityFramework.Shared.DbContexts
{
    public class IdentityServerDataProtectionDbContext : DbContext, IDataProtectionKeyContext
    {
        public IdentityServerDataProtectionDbContext(DbContextOptions<IdentityServerDataProtectionDbContext> options)
            : base(options) { }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}