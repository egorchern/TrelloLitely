using Microsoft.EntityFrameworkCore;
using EzyClassroomz.Library.Entities;
using System.ComponentModel.DataAnnotations;

namespace EzyClassroomz.Library.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => new { e.Email, e.Name }).IsUnique();
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.TenantId);
            });
        }
    }
}
