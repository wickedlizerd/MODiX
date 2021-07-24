using Microsoft.Extensions.Logging;

namespace Modix.Data.Diagnostics
{
    internal static partial class DiagnosticsLogMessages
    {
        [LoggerMessage(
            EventId = 0x4ADDBF7B,
            Level   = LogLevel.Warning,
            Message = "Data connection health check failed")]
        public static partial void ConnectionHealthCheckFailed(ILogger logger);

        [LoggerMessage(
            EventId = 0x118D16D7,
            Level   = LogLevel.Debug,
            Message = "Checking data connection health")]
        public static partial void ConnectionHealthChecking(ILogger logger);

        [LoggerMessage(
            EventId = 0x623352A7,
            Level   = LogLevel.Debug,
            Message = "Data connection health check succeeded")]
        public static partial void ConnectionHealthCheckSucceeded(ILogger logger);
    }
}
