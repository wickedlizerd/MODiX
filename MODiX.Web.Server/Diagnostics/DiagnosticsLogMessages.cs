using Microsoft.Extensions.Logging;

namespace Modix.Web.Server.Diagnostics
{
    internal static partial class DiagnosticsLogMessages
    {
        [LoggerMessage(
            EventId = 0x756A1258,
            Level   = LogLevel.Debug,
            Message = "Web Server pipeline health check performed")]
        public static partial void PipelineHealthCheckPerformed(ILogger logger);
    }
}
