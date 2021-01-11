using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace Modix.Business.Diagnostics
{
    public interface IDiagnosticsService
    {
        int PingTestEndpointCount { get; }

        Task<IReadOnlyList<PingTestOutcome>> PerformPingTestAsync(CancellationToken cancellationToken);
    }

    public class DiagnosticsService
        : IDiagnosticsService
    {
        public const int DefaultTimeout = 5000;

        public DiagnosticsService(
            IOptions<DiagnosticsConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public int PingTestEndpointCount
            => _configuration.Value.PingEndpointsByName?.Count ?? 0;

        public async Task<IReadOnlyList<PingTestOutcome>> PerformPingTestAsync(CancellationToken cancellationToken)
            => _configuration.Value.PingEndpointsByName is not null
                ? await Task.WhenAll(_configuration.Value.PingEndpointsByName
                    .Select(endpointByName => PingEndpointAsync(endpointByName.Key, endpointByName.Value, cancellationToken))
                    .ToArray())
                : Array.Empty<PingTestOutcome>();

        private async Task<PingTestOutcome> PingEndpointAsync(
            string endpointName,
            string endpoint,
            CancellationToken cancellationToken)
        {
            using var ping = new Ping();

            var pingOperation = ping.SendPingAsync(endpoint, (int)(_configuration.Value.PingTimeout?.TotalMilliseconds ?? DefaultTimeout));

            cancellationToken.Register(ping.SendAsyncCancel);

            try
            {
                var result = await pingOperation;

                return new PingTestOutcome(
                    endpointName:   endpointName,
                    latency:        (result?.Status != IPStatus.TimedOut)
                        ? TimeSpan.FromMilliseconds(result!.RoundtripTime)
                        : null,
                    status:         result?.Status ?? IPStatus.Unknown);
            }
            catch
            {
                return new PingTestOutcome(
                    endpointName:   endpointName,
                    latency:        null,
                    status:         IPStatus.Unknown);
            }
        }

        private readonly IOptions<DiagnosticsConfiguration> _configuration;
    }
}
