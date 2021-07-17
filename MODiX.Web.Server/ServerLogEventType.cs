using Modix.Common;

namespace Modix.Web.Server
{
    internal enum ServerLogEventType
    {
        Authentication  = ModixLogEventType.Server + 0x010000,
        Authorization   = ModixLogEventType.Server + 0x020000
    }
}
