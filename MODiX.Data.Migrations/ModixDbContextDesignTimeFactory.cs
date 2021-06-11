using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modix.Data.Migrations
{
    internal class ModixDbContextDesignTimeFactory
        : IDesignTimeDbContextFactory<ModixDbContext>
    {
        public ModixDbContext CreateDbContext(string[] args)
            => new ServiceCollection()
                .AddLogging()
                .AddModixData(new ConfigurationBuilder()
                    .AddUserSecrets<ModixDbContext>()
                    .Build())
                .BuildServiceProvider()
                .GetRequiredService<ModixDbContext>();
    }
}
