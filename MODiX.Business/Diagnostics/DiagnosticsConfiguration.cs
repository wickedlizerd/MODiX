using System;
using System.Collections.Generic;

namespace Modix.Business.Diagnostics
{
    public class DiagnosticsConfiguration
    {
        public Dictionary<string, string>? PingTestEndpointsByName { get; set; }

        public TimeSpan? PingTestTimeout { get; set; }
    }
}
