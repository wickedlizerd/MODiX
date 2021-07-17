using System;
using System.Net.NetworkInformation;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

namespace Modix.Business.Diagnostics
{
    internal static class DiagnosticsLogMessages
    {
        private enum LogEvent
        {
            SystemClockStarting             = BusinessLogEventType.Diagnostics + 0x0101,
            SystemClockStopping             = BusinessLogEventType.Diagnostics + 0x0102,
            PingTestEndpointsNotConfigured  = BusinessLogEventType.Diagnostics + 0x0201,
            PingTestPerforming              = BusinessLogEventType.Diagnostics + 0x0202,
            PingTestPerformed               = BusinessLogEventType.Diagnostics + 0x0203,
            PingTestEndpointPinging         = BusinessLogEventType.Diagnostics + 0x0204,
            PingTestEndpointPinged          = BusinessLogEventType.Diagnostics + 0x0205,
            PingTestEndpointPingFailed      = BusinessLogEventType.Diagnostics + 0x0206
        }

        public static void PingTestEndpointPinged(
                ILogger     logger,
                string      endpointName,
                string      endpoint,
                IPStatus?    status,
                long?        roundtripTime)
            => _pingTestEndpointPinged.Invoke(
                logger,
                endpointName,
                endpoint,
                status,
                roundtripTime);
        private static readonly Action<ILogger, string, string, IPStatus?, long?> _pingTestEndpointPinged
            = LoggerMessage.Define<string, string, IPStatus?, long?>(
                    LogLevel.Debug,
                    LogEvent.PingTestEndpointPinged.ToEventId(),
                    "Endpoint \"{EndpointName}\" ({Endpoint}) pinged: {Status}, {RoundtripTime}")
                .WithoutException();

        public static void PingTestEndpointPingFailed(
                ILogger     logger,
                string      endpointName,
                string      endpoint,
                Exception   exception)
            => _pingTestEndpointPingFailed.Invoke(
                logger,
                endpointName,
                endpoint,
                exception);
        private static readonly Action<ILogger, string, string, Exception?> _pingTestEndpointPingFailed
            = LoggerMessage.Define<string, string>(
                    LogLevel.Error,
                    LogEvent.PingTestEndpointPingFailed.ToEventId(),
                    "Endpoint \"{EndpointName}\" ({Endpoint}) ping failed");

        public static void PingTestEndpointPinging(
                ILogger     logger,
                string      endpointName,
                string      endpoint,
                TimeSpan    timeout)
            => _pingTestEndpointPinging.Invoke(
                logger,
                endpointName,
                endpoint,
                timeout);
        private static readonly Action<ILogger, string, string, TimeSpan> _pingTestEndpointPinging
            = StructuredLoggerMessage.Define<string, string, TimeSpan>(
                    LogLevel.Debug,
                    LogEvent.PingTestEndpointPinging.ToEventId(),
                    "Pinging endpoint \"{EndpointName}\" ({Endpoint})",
                    "Timeout")
                .WithoutException();

        public static void PingTestEndpointsNotConfigured(ILogger logger)
            => _pingTestEndpointsNotConfigured.Invoke(logger);
        private static readonly Action<ILogger> _pingTestEndpointsNotConfigured
            = LoggerMessage.Define(
                    LogLevel.Warning,
                    LogEvent.PingTestEndpointsNotConfigured.ToEventId(),
                    "No ping test endpoints have been configured")
                .WithoutException();

        public static void PingTestPerformed(ILogger logger)
            => _pingTestPerformed.Invoke(logger);
        private static readonly Action<ILogger> _pingTestPerformed
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    LogEvent.PingTestPerformed.ToEventId(),
                    "Ping test performed")
                .WithoutException();

        public static void PingTestPerforming(
                ILogger logger,
                int     endpointCount)
            => _pingTestPerforming.Invoke(
                logger,
                endpointCount);
        private static readonly Action<ILogger, int> _pingTestPerforming
            = LoggerMessage.Define<int>(
                    LogLevel.Debug,
                    LogEvent.PingTestPerforming.ToEventId(),
                    "Performing Ping Test ({EndpointCount} endpoints)")
                .WithoutException();

        public static void SystemClockStarting(ILogger logger)
            => _systemClockStarting.Invoke(logger);
        private static readonly Action<ILogger> _systemClockStarting
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    LogEvent.SystemClockStarting.ToEventId(),
                    "Starting system clock diagnostics")
                .WithoutException();

        public static void SystemClockStopping(ILogger logger)
            => _systemClockStopping.Invoke(logger);
        private static readonly Action<ILogger> _systemClockStopping
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    LogEvent.SystemClockStopping.ToEventId(),
                    "Stopping system clock diagnostics")
                .WithoutException();
    }
}
