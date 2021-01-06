using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Minify.DAL
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args = null)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(GetConnectionString(), providerOptions => providerOptions.CommandTimeout(60))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new AppDbContext(builder.Options);
        }

        /// <summary>
        /// Gets the connection string based on the environment setting in appsettings.json
        /// </summary>
        /// <returns>The connection string</returns>
        public string GetConnectionString()
        {
            IConfigurationBuilder configbuilder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = configbuilder.Build();

            string connectionStringName = configuration.GetSection("Environment")["IsDevelopment"] == "false" ? "Production" : "Development";

            return configuration.GetConnectionString(connectionStringName);
        }
    }
}