using ProtoBuf;

namespace Modix.Web.Protocol.Diagnostics
{
    [ProtoContract]
    [ProtoInclude(1, typeof(PingTestDefinitions))]
    [ProtoInclude(2, typeof(PingTestOutcome))]
    public abstract class PingTestResponse { }
}
