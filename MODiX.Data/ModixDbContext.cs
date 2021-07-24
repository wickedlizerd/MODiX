using System.Reflection;

using Microsoft.EntityFrameworkCore;

namespace Modix.Data
{
    internal class ModixDbContext
        : DbContext
    {
        public ModixDbContext(DbContextOptions<ModixDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
