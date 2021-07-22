using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Modix.Data.Diagnostics
{
    internal class ConnectionHealthCheck
        : IHealthCheck
    {
        public ConnectionHealthCheck(
            ILogger<ConnectionHealthCheck>   logger,
            ModixDbContext                  modixDbContext)
        {
            _logger         = logger;
            _modixDbContext = modixDbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
                HealthCheckContext  context,
                CancellationToken   cancellationToken)
        {
            DiagnosticsLogMessages.ConnectionHealthChecking(_logger);
            if(await _modixDbContext.Database.CanConnectAsync(cancellationToken))
            {
                DiagnosticsLogMessages.ConnectionHealthCheckSucceeded(_logger);
                return HealthCheckResult.Healthy();
            }
            else
            {
                DiagnosticsLogMessages.ConnectionHealthCheckFailed(_logger);
                return new(context.Registration.FailureStatus);
            }
        }

        private readonly ILogger        _logger;
        private readonly ModixDbContext _modixDbContext;
    }
}
