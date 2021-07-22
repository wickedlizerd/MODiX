using System;

using Microsoft.Extensions.Logging;

namespace Modix.Data.Diagnostics
{
    internal static class DiagnosticsLogMessages
    {
        private enum EventType
        {
            ConnectionHealthChecking        = DataLogEventType.Diagnostics + 0x0100,
            ConnectionHealthCheckFailed     = DataLogEventType.Diagnostics + 0x0200,
            ConnectionHealthCheckSucceeded  = DataLogEventType.Diagnostics + 0x0300
        }

        public static void ConnectionHealthCheckFailed(ILogger logger)
            => _connectionHealthCheckFailed.Invoke(logger);
        private static readonly Action<ILogger> _connectionHealthCheckFailed
            = LoggerMessage.Define(
                    LogLevel.Warning,
                    EventType.ConnectionHealthCheckFailed.ToEventId(),
                    "Data connection health check failed")
                .WithoutException();

        public static void ConnectionHealthChecking(ILogger logger)
            => _connectionHealthChecking.Invoke(logger);
        private static readonly Action<ILogger> _connectionHealthChecking
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.ConnectionHealthChecking.ToEventId(),
                    "Checking data connection health")
                .WithoutException();

        public static void ConnectionHealthCheckSucceeded(ILogger logger)
            => _connectionHealthCheckSucceeded.Invoke(logger);
        private static readonly Action<ILogger> _connectionHealthCheckSucceeded
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.ConnectionHealthCheckSucceeded.ToEventId(),
                    "Data connection health check succeeded")
                .WithoutException();
    }
}
