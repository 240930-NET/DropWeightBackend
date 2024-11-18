using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DropWeightBackend.Infrastructure.Data
{
    public class DropWeightContextFactory : IDesignTimeDbContextFactory<DropWeightContext>
    {
        public DropWeightContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), @"..\DropWeightBackend.Api\appsettings.json"))
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DropWeightContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new DropWeightContext(optionsBuilder.Options);
        }
    }
}
