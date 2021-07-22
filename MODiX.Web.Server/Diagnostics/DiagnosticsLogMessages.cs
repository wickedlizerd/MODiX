using System;

using Microsoft.Extensions.Logging;

namespace Modix.Web.Server.Diagnostics
{
    internal static class DiagnosticsLogMessages
    {
        private enum EventType
        {
            PipelineHealthCheckPerformed    = ServerLogEventType.Diagnostics + 0x0100
        }

        public static void PipelineHealthCheckPerformed(ILogger logger)
            => _pipelineHealthCheckPerformed.Invoke(logger);
        private static readonly Action<ILogger> _pipelineHealthCheckPerformed
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.PipelineHealthCheckPerformed.ToEventId(),
                    "Web Server pipeline health check performed")
                .WithoutException();
    }
}
