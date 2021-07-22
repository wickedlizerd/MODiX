using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Modix.Web.Server.Diagnostics
{
    public class PipelineHealthCheck
        : IHealthCheck
    {
        public PipelineHealthCheck(ILogger<PipelineHealthCheck> logger)
            => _logger = logger;

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext  context,
            CancellationToken   cancellationToken)
        {
            _logger.LogDebug("Web server pipeline health check performed");
            return Task.FromResult(HealthCheckResult.Healthy());
        }

        private readonly ILogger _logger;
    }
}
