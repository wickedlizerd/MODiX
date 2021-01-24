using System;

using ProtoBuf;

namespace Modix.Web.Protocol.Diagnostics
{
    [ProtoContract]
    public class PingTestOutcome
        : PingTestResponse
    {
        public PingTestOutcome(
            string endpointName,
            TimeSpan? latency,
            EndpointStatus status)
        {
            EndpointName = endpointName;
            Latency = latency;
            Status = status;
        }

        // Private constructor and initters are needed for deserialization
        private PingTestOutcome()
            => EndpointName = null!;

        [ProtoMember(1)]
        public string EndpointName { get; private init; }

        [ProtoMember(2, DataFormat = DataFormat.WellKnown)]
        public TimeSpan? Latency { get; private init; }

        [ProtoMember(3)]
        public EndpointStatus Status { get; private init; }
    }
}
