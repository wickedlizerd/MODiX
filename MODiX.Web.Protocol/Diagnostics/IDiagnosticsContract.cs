using System.Collections.Generic;
using System.ServiceModel;

namespace Modix.Web.Protocol.Diagnostics
{
    [ServiceContract(Name = "Diagnostics")]
    public interface IDiagnosticsContract
    {
        [OperationContract(Name = "ObserveSystemClock")]
        IAsyncEnumerable<SystemClockResponse> ObserveSystemClock();

        [OperationContract(Name = "PerformPingTest")]
        IAsyncEnumerable<PingTestResponse> PerformPingTest();
    }
}
