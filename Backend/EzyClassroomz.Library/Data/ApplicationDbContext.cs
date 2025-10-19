using Microsoft.EntityFrameworkCore;
using EzyClassroomz.Library.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace EzyClassroomz.Library.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAuthorizationPolicy> UserAuthorizationPolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.TenantId);
                entity
                    .HasMany(e => e.AuthorizationPolicies)
                    .WithOne(p => p.User)
                    .HasForeignKey(p => p.UserId);
            });

            modelBuilder.Entity<UserAuthorizationPolicy>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.UserId }).IsUnique();
            });
        }
    }
}
