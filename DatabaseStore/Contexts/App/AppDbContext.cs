using AspNetFlex.DatabaseStore.Contexts.App.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetFlex.DatabaseStore.Contexts.App
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserDbEntity> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyProperties.Assembly);
        }
    }
}