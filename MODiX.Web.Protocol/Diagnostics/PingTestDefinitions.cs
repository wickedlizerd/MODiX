using System.Collections.Immutable;

using ProtoBuf;

namespace Modix.Web.Protocol.Diagnostics
{
    [ProtoContract]
    public class PingTestDefinitions
        : PingTestResponse
    {
        [ProtoMember(1)]
        public ImmutableArray<string> EndpointNames { get; init; }
    }
}
