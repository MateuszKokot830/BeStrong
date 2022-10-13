using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext() 
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.Development.json");
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<AppUser> Users { get; set; }
    }
}