using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Modix.Data.Migrations
{
    public class ModixDbAutoMigrationStartupAction
        : StartupActionBase
    {
        public ModixDbAutoMigrationStartupAction(
                    ILogger<ModixDbAutoMigrationStartupAction>  logger,
                    IServiceScopeFactory                        serviceScopeFactory)
                : base(logger)
            => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task OnStartupAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();

            MigrationsLogMessages.DatabaseMigrating(Logger);
            await serviceScope.ServiceProvider.GetRequiredService<ModixDbContext>().Database.MigrateAsync(cancellationToken);
            MigrationsLogMessages.DatabaseMigrated(Logger);
        }

        private readonly IServiceScopeFactory _serviceScopeFactory;
    }
}
