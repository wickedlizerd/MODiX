using System;

using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web.Client.Diagnostics
{
    public class PingTestState
        : PingTestOutcome
    {
        public PingTestState(
                    string endpointName,
                    bool hasCompleted,
                    TimeSpan? latency,
                    EndpointStatus status)
                : base(
                    endpointName,
                    latency,
                    status)
            => HasCompleted = hasCompleted;

        public bool HasCompleted { get; init; }
    }
}
