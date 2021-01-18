using System;

namespace Modix.Business.Diagnostics
{
    public static class DiagnosticsDefaults
    {
        public static readonly TimeSpan DefaultPingTestTimeout
            = TimeSpan.FromSeconds(5);
    }
}
