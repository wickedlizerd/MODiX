using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using Modix.Business.Diagnostics;
using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web.Server.Diagnostics
{
    public class DiagnosticsContract
        : IDiagnosticsContract
    {
        public DiagnosticsContract(IDiagnosticsService diagnosticsService)
            => _diagnosticsService = diagnosticsService;

        public IAsyncEnumerable<PingTestResponse> PerformPingTest()
            => AsyncEnumerable.Empty<PingTestResponse>()
                .Append(new PingTestDefinitions()
                {
                    EndpointNames = _diagnosticsService.PingTestEndpointNames
                })
                .Concat(_diagnosticsService.PerformPingTest());

        private readonly IDiagnosticsService _diagnosticsService;
    }
}
