using System;
using System.Runtime.CompilerServices;

using ProtoBuf.Meta;

namespace Modix.Web.Protocol
{
    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Initialize()
            => RuntimeTypeModel.Default
                .Add(typeof(DateTimeOffset), applyDefaultBehaviour: false)
                .SetSurrogate(typeof(DateTimeOffsetModel));
    }
}
