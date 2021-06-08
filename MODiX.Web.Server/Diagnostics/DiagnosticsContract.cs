using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Microsoft.AspNetCore.Authorization;

using Modix.Business.Diagnostics;
using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web.Server.Diagnostics
{
    [Authorize]
    public class DiagnosticsContract
        : IDiagnosticsContract
    {
        public DiagnosticsContract(
            IDiagnosticsManager diagnosticsManager,
            IDiagnosticsService diagnosticsService)
        {
            _diagnosticsManager = diagnosticsManager;
            _diagnosticsService = diagnosticsService;
        }

        public IAsyncEnumerable<SystemClockResponse> ObserveSystemClock()
            => _diagnosticsManager.Now
                .Select(now => new SystemClockResponse()
                {
                    Now = now
                })
                .ToAsyncEnumerable()
                .WithSilentCancellation();

        public IAsyncEnumerable<PingTestResponse> PerformPingTest()
            => AsyncEnumerable.Empty<PingTestResponse>()
                .Append(new PingTestDefinitions()
                {
                    EndpointNames = _diagnosticsService.PingTestEndpointNames
                })
                .Concat(_diagnosticsService.PerformPingTest());

        private readonly IDiagnosticsManager _diagnosticsManager;
        private readonly IDiagnosticsService _diagnosticsService;
    }
}
