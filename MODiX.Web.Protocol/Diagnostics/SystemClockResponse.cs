using System;

using ProtoBuf;

namespace Modix.Web.Protocol.Diagnostics
{
    [ProtoContract]
    public class SystemClockResponse
    {
        [ProtoMember(1)]
        public DateTimeOffset Now { get; init; }
    }
}
