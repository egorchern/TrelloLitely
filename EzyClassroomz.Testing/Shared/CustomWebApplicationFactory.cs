using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzyClassroomz.Library.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EzyClassroomz.Testing.Shared
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly SqliteConnection _connection;

        public CustomWebApplicationFactory()
        {
            // keep connection open for lifetime of factory so in-memory SQLite DB persists
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }
    
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove ALL DbContext-related service descriptors including the internal service provider
                var descriptorsToRemove = services
                    .Where(d => 
                        d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                        d.ServiceType == typeof(DbContextOptions) ||
                        d.ServiceType == typeof(ApplicationDbContext))
                    .ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                // Manually add DbContext with SQLite - this avoids the internal service provider issue
                services.AddScoped<ApplicationDbContext>(sp =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    optionsBuilder.UseSqlite(_connection);
                    return new ApplicationDbContext(optionsBuilder.Options);
                });
            });

            builder.UseEnvironment("Testing");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            base.Dispose(disposing);
        }

        public void InitializeDatabase()
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
        }
    }
}
