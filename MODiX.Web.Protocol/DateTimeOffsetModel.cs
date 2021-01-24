using System;
using ProtoBuf;

namespace Modix.Web.Protocol
{
    [ProtoContract]
    public class DateTimeOffsetModel
    {
        [ProtoMember(1)]
        public long Ticks { get; init; }

        [ProtoMember(2)]
        public short OffsetMinutes { get; init; }

        public static implicit operator DateTimeOffsetModel(DateTimeOffset value)
            => new()
            {
                Ticks = value.Ticks,
                OffsetMinutes = (short)value.Offset.TotalMinutes
            };

        public static implicit operator DateTimeOffset(DateTimeOffsetModel value)
            => new(value.Ticks, TimeSpan.FromMinutes(value.OffsetMinutes));
    }
}
