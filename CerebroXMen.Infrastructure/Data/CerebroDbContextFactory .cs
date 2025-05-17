using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CerebroXMen.Infrastructure.Data
{
    public class CerebroDbContextFactory : IDesignTimeDbContextFactory<CerebroDbContext>
    {
        public CerebroDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CerebroXMen.API")) 
                .AddJsonFile("appsettings.json") 
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CerebroDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new CerebroDbContext(optionsBuilder.Options);
        }
    }
}
