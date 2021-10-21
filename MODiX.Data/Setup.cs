using System.Transactions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Modix.Data.Diagnostics;
using Modix.Data.Permissions;
using Modix.Data.Users;

namespace Modix.Data
{
    public static class Setup
    {
        public static IServiceCollection AddModixData(this IServiceCollection services, IConfiguration configuration)
            => services
                .Add(services => services.AddOptions<DataConfiguration>()
                    .Bind(configuration.GetSection("MODiX:Data"))
                    .ValidateDataAnnotations()
                    .ValidateOnStartup())
                .AddDbContext<ModixDbContext>((serviceProvider, builder) => builder
                    .ConfigureWarnings(builder => builder.Ignore(CoreEventId.SaveChangesFailed)) // Workaround: https://github.com/dotnet/efcore/issues/26417
                    .UseNpgsql(
                        serviceProvider.GetRequiredService<IOptions<DataConfiguration>>().Value.ConnectionString,
                        optionsBuilder => optionsBuilder
                            .MigrationsAssembly("MODiX.Data.Migrations")))
                .AddSingleton<ITransactionScopeFactory, TransactionScopeFactory>()
                .AddDiagnostics()
                .AddPermissions()
                .AddUsers();
    }
}
