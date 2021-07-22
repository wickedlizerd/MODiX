using Microsoft.Extensions.DependencyInjection;

namespace Modix.Data.Diagnostics
{
    public static class Setup
    {
        public static IServiceCollection AddDiagnostics(this IServiceCollection services)
            => services
                .Add(services => services.AddHealthChecks()
                    .AddCheck<ConnectionHealthCheck>("modix-data-connection"));
    }
}
