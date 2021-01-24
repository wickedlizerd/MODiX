using System;

using ProtoBuf.Meta;

namespace Modix.Web.Protocol
{
    public static class ProtocolConfiguration
    {
        public static void Apply()
            => RuntimeTypeModel.Default
                .Add(typeof(DateTimeOffset), applyDefaultBehaviour: false)
                .SetSurrogate(typeof(DateTimeOffsetModel));
    }
}
