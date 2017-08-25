using Microsoft.EntityFrameworkCore;
using Entities = Toggle.Domain.Entities;

namespace Toggle.Infra
{
    public class ToggleContext : DbContext
    {
        public ToggleContext(DbContextOptions options) : base(options) { }

        public DbSet<Entities.Service> Services { get; set; }
        public DbSet<Entities.Toggle> Toggles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Service>()
                .Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Entities.Toggle>()
                .Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Entities.Toggle>()
                .Property(t => t.Value).IsRequired();
        }
    }
}
