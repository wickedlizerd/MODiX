using System.Transactions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Modix.Data.Permissions;
using Modix.Data.Users;

namespace Modix.Data
{
    public static class Setup
    {
        public static IServiceCollection AddModixData(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDbContext<ModixDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("MODiX.Data"), optionsBuilder => optionsBuilder
                        .MigrationsAssembly("MODiX.Data.Migrations")))
                .AddSingleton<ITransactionScopeFactory, TransactionScopeFactory>()
                .AddPermissions()
                .AddUsers();
    }
}
