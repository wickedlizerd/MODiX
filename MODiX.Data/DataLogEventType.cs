using Modix.Common;

namespace Modix.Data
{
    public enum DataLogEventType
    {
        Migrations      = ModixLogEventType.Data + 0x010000,
        Transactions    = ModixLogEventType.Data + 0x020000,
        Auditing        = ModixLogEventType.Data + 0x030000,
        Permissions     = ModixLogEventType.Data + 0x040000,
        Users           = ModixLogEventType.Data + 0x050000
    }
}
