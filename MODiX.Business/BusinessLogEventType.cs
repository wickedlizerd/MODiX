using Modix.Common;

namespace Modix.Business
{
    public enum BusinessLogEventType
    {
        Caching         = ModixLogEventType.Business + 0x010000,
        Messaging       = ModixLogEventType.Business + 0x020000,
        Authorization   = ModixLogEventType.Business + 0x030000,
        Users           = ModixLogEventType.Business + 0x040000,
        Guilds          = ModixLogEventType.Business + 0x050000,
        Diagnostics     = ModixLogEventType.Business + 0x060000
    }
}
