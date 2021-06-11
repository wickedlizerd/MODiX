using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modix.Data.Migrations
{
    public static class Setup
    {
        public static IServiceCollection AddModixDataMigrations(this IServiceCollection services)
            => services.AddStartupAction<ModixDbAutoMigrationStartupAction>();
    }
}
