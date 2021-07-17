using Modix.Common;

namespace Modix.Business
{
    public enum BusinessLogEventType
    {
        Caching         = ModixLogEventType.Business + 0x010000,
        Hosting         = ModixLogEventType.Business + 0x020000,
        GatewayReaction = ModixLogEventType.Business + 0x030000,
        Authorization   = ModixLogEventType.Business + 0x040000,
        Users           = ModixLogEventType.Business + 0x050000,
        Guilds          = ModixLogEventType.Business + 0x060000,
        Diagnostics     = ModixLogEventType.Business + 0x070000
    }
}
