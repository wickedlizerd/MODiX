using System;
using System.Net.NetworkInformation;

using Microsoft.Extensions.Logging;

namespace Modix.Business.Diagnostics
{
    internal static partial class DiagnosticsLogMessages
    {
        [LoggerMessage(
            EventId = 0x5AAF9778,
            Level   = LogLevel.Debug,
            Message = "Endpoint [{EndpointName}] ({Endpoint}) pinged: {Status}, {RoundtripTime}ms")]
        public static partial void PingTestEndpointPinged(
            ILogger     logger,
            string      endpointName,
            string      endpoint,
            IPStatus?   status,
            long?       roundtripTime);

        [LoggerMessage(
            EventId = 0x4CA713AB,
            Level   = LogLevel.Debug,
            Message = "Endpoint [{EndpointName}] ({Endpoint}) ping failed")]
        public static partial void PingTestEndpointPingFailed(
            ILogger     logger,
            string      endpointName,
            string      endpoint,
            Exception   exception);

        [LoggerMessage(
            EventId = 0x68F015C4,
            Level   = LogLevel.Debug,
            Message = "Pinging endpoint [{EndpointName}] ({Endpoint})")]
        public static partial void PingTestEndpointPinging(
            ILogger     logger,
            string      endpointName,
            string      endpoint,
            TimeSpan    timeout);

        [LoggerMessage(
            EventId = 0x1805E0CB,
            Level   = LogLevel.Warning,
            Message = "No ping test endpoints have been configured")]
        public static partial void PingTestEndpointsNotConfigured(ILogger logger);

        [LoggerMessage(
            EventId = 0x4E0161AD,
            Level   = LogLevel.Debug,
            Message = "Ping test performed")]
        public static partial void PingTestPerformed(ILogger logger);

        [LoggerMessage(
            EventId = 0x661F18A4,
            Level   = LogLevel.Debug,
            Message = "Performing Ping Test ({EndpointCount} endpoints)")]
        public static partial void PingTestPerforming(
            ILogger logger,
            int     endpointCount);

        [LoggerMessage(
            EventId = 0x2B5DE3E7,
            Level   = LogLevel.Debug,
            Message = "Starting system clock diagnostics")]
        public static partial void SystemClockStarting(ILogger logger);

        [LoggerMessage(
            EventId = 0x529EB71D,
            Level   = LogLevel.Debug,
            Message = "Stopping system clock diagnostics")]
        public static partial void SystemClockStopping(ILogger logger);
    }
}
