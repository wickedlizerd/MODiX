using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Modix.Data
{
    internal class ModixDbContext
        : DbContext
    {
        public ModixDbContext(IOptions<DataConfiguration> configuration)
            => _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(
                _configuration.Value.ConnectionString,
                optionsBuilder => optionsBuilder
                    .MigrationsAssembly("MODiX.Data.Migrations"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        private readonly IOptions<DataConfiguration> _configuration;
    }
}
