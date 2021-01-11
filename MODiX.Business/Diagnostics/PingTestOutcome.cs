using System;
using System.Net.NetworkInformation;

namespace Modix.Business.Diagnostics
{
    public class PingTestOutcome
    {
        public PingTestOutcome(
            string endpointName,
            TimeSpan? latency,
            IPStatus status)
        {
            EndpointName = endpointName;
            Latency = latency;
            Status = status;
        }

        public string EndpointName { get; }

        public TimeSpan? Latency { get; }

        public IPStatus Status { get; }
    }
}
