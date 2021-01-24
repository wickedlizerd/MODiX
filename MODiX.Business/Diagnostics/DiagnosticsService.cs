using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public DiagnosticsService(IOptions<DiagnosticsConfiguration> configuration)
            => _configuration = configuration;

        public ImmutableArray<string> PingTestEndpointNames
            => _configuration.Value.PingTestEndpointsByName?.Keys.ToImmutableArray()
                ?? ImmutableArray<string>.Empty;

        public IAsyncEnumerable<PingTestOutcome> PerformPingTest()
            => _configuration.Value.PingTestEndpointsByName?
                    .Select(endpointByName => Observable.FromAsync(cancellationToken => PingEndpointAsync(endpointByName.Key, endpointByName.Value, cancellationToken)))
                    .Merge()
                    .ToAsyncEnumerable()
                ?? AsyncEnumerable.Empty<PingTestOutcome>();

        private async Task<PingTestOutcome> PingEndpointAsync(
            string endpointName,
            string endpoint,
            CancellationToken cancellationToken)
        {
            using var ping = new Ping();

            var pingOperation = ping.SendPingAsync(endpoint, (int)(_configuration.Value.PingTestTimeout?.TotalMilliseconds ?? DiagnosticsDefaults.DefaultPingTestTimeout.TotalMilliseconds));

            cancellationToken.Register(ping.SendAsyncCancel);

            try
            {
                var result = await pingOperation;

                return new PingTestOutcome(
                    endpointName:   endpointName,
                    latency:        (result?.Status != IPStatus.TimedOut)
                        ? TimeSpan.FromMilliseconds(result!.RoundtripTime)
                        : null,
                    status:         (EndpointStatus)(result?.Status ?? IPStatus.Unknown));
            }
            catch
            {
                return new PingTestOutcome(
                    endpointName:   endpointName,
                    latency:        null,
                    status:         EndpointStatus.Unknown);
            }
        }

        private readonly IOptions<DiagnosticsConfiguration> _configuration;
    }
}
