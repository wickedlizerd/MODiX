using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Modix.Web.Protocol.Diagnostics;

namespace Modix.Business.Diagnostics
{
    public interface IDiagnosticsService
    {
        ImmutableArray<string> PingTestEndpointNames { get; }

        IAsyncEnumerable<PingTestOutcome> PerformPingTest();
    }

    internal class DiagnosticsService
        : IDiagnosticsService
    {
        public DiagnosticsService(
            IOptions<DiagnosticsConfiguration>  configuration,
            ILogger<DiagnosticsService>         logger)
        {
            _configuration  = configuration;
            _logger         = logger;
        }

        public ImmutableArray<string> PingTestEndpointNames
            => _configuration.Value.PingTestEndpointsByName?.Keys.ToImmutableArray()
                ?? ImmutableArray<string>.Empty;

        public IAsyncEnumerable<PingTestOutcome> PerformPingTest()
        {
            if (_configuration.Value.PingTestEndpointsByName is null or { Count: 0 })
            {
                DiagnosticsLogMessages.PingTestEndpointsNotConfigured(_logger);
                return AsyncEnumerable.Empty<PingTestOutcome>();
            }

            DiagnosticsLogMessages.PingTestPerforming(_logger, _configuration.Value.PingTestEndpointsByName.Count);

            var timeout = _configuration.Value.PingTestTimeout ?? DiagnosticsDefaults.DefaultPingTestTimeout;

            return _configuration.Value.PingTestEndpointsByName
                .Select(endpointByName => Observable.FromAsync(cancellationToken => PingEndpointAsync(endpointByName.Key, endpointByName.Value, timeout, cancellationToken)))
                .Merge()
                .Finally(() => DiagnosticsLogMessages.PingTestPerformed(_logger))
                .ToAsyncEnumerable();
        }

        private async Task<PingTestOutcome> PingEndpointAsync(
            string              endpointName,
            string              endpoint,
            TimeSpan            timeout,
            CancellationToken   cancellationToken)
        {
            DiagnosticsLogMessages.PingTestEndpointPinging(_logger, endpointName, endpoint, timeout);

            using var ping = new Ping();

            var pingOperation = ping.SendPingAsync(endpoint);

            cancellationToken.Register(ping.SendAsyncCancel);

            try
            {
                var result = await pingOperation;

                DiagnosticsLogMessages.PingTestEndpointPinged(_logger, endpointName, endpoint, result?.Status, result?.RoundtripTime);

                return new PingTestOutcome(
                    endpointName:   endpointName,
                    latency:        (result?.Status != IPStatus.TimedOut)
                        ? TimeSpan.FromMilliseconds(result!.RoundtripTime)
                        : null,
                    status:         (EndpointStatus)(result?.Status ?? IPStatus.Unknown));
            }
            catch(Exception ex)
            {
                DiagnosticsLogMessages.PingTestEndpointPingFailed(_logger, endpointName, endpoint, ex);

                return new PingTestOutcome(
                    endpointName:   endpointName,
                    latency:        null,
                    status:         EndpointStatus.Unknown);
            }
        }

        private readonly IOptions<DiagnosticsConfiguration> _configuration;
        private readonly ILogger                            _logger;
    }
}
