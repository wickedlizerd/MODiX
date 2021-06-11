using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modix.Data.Migrations
{
    public class ModixDbAutoMigrationStartupAction
        : StartupActionBase
    {
        public ModixDbAutoMigrationStartupAction(IServiceScopeFactory serviceScopeFactory)
            => _serviceScopeFactory = serviceScopeFactory;

        protected override async Task OnStartupAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();

            await serviceScope.ServiceProvider.GetRequiredService<ModixDbContext>().Database.MigrateAsync(cancellationToken);
        }

        private readonly IServiceScopeFactory _serviceScopeFactory;
    }
}
