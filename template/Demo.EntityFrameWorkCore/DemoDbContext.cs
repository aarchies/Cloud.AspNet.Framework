using Microsoft.EntityFrameworkCore;

namespace Demo.EntityFrameWorkCore
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Model.Demo> PersonSyncStates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Model.Demo>().ToTable("PersonSyncState");
        }
    }
}
