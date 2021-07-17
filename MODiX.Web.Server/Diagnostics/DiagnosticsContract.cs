using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Modix.Business.Diagnostics;
using Modix.Data.Administration;
using Modix.Web.Protocol.Diagnostics;
using Modix.Web.Server.Authorization;

namespace Modix.Web.Server.Diagnostics
{
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

        [RequirePermissions(AdministrationPermission.DiagnosticsRead)]
        public IAsyncEnumerable<SystemClockResponse> ObserveSystemClock()
            => _diagnosticsManager.SystemClock
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
