using System;
using System.Collections.Generic;

namespace Modix.Business.Diagnostics
{
    public class DiagnosticsConfiguration
    {
        public Dictionary<string, string>? PingEndpointsByName { get; set; }

        public TimeSpan? PingTimeout { get; set; }
    }
}
