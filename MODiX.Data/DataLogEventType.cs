using Modix.Common;

namespace Modix.Data
{
    public enum DataLogEventType
    {
        Diagnostics     = ModixLogEventType.Data + 0x010000,
        Migrations      = ModixLogEventType.Data + 0x020000,
        Transactions    = ModixLogEventType.Data + 0x030000,
        Auditing        = ModixLogEventType.Data + 0x040000,
        Permissions     = ModixLogEventType.Data + 0x050000,
        Users           = ModixLogEventType.Data + 0x060000
    }
}
