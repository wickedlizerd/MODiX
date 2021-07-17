using System;

using Microsoft.Extensions.Logging;

using Modix.Business;

namespace Microsoft.Extensions.Hosting
{
    internal static class HostingLogMessages
    {
        private enum EventType
        {
            BehaviorStarting    = BusinessLogEventType.Hosting + 0x0100,
            BehaviorStarted     = BusinessLogEventType.Hosting + 0x0200,
            BehaviorStopping    = BusinessLogEventType.Hosting + 0x0300,
            BehaviorStopped     = BusinessLogEventType.Hosting + 0x0400
        }

        public static void BehaviorStarted(ILogger logger)
            => _behaviorStarted.Invoke(logger);
        private static readonly Action<ILogger> _behaviorStarted
            = LoggerMessage.Define(
                    LogLevel.Information,
                    EventType.BehaviorStarted.ToEventId(),
                    "Behavior started")
                .WithoutException();

        public static void BehaviorStarting(ILogger logger)
            => _behaviorStarting.Invoke(logger);
        private static readonly Action<ILogger> _behaviorStarting
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.BehaviorStarting.ToEventId(),
                    "Starting behavior")
                .WithoutException();

        public static void BehaviorStopped(ILogger logger)
            => _behaviorStopped.Invoke(logger);
        private static readonly Action<ILogger> _behaviorStopped
            = LoggerMessage.Define(
                    LogLevel.Information,
                    EventType.BehaviorStopped.ToEventId(),
                    "Behavior stopped")
                .WithoutException();

        public static void BehaviorStopping(ILogger logger)
            => _behaviorStopping.Invoke(logger);
        private static readonly Action<ILogger> _behaviorStopping
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.BehaviorStopping.ToEventId(),
                    "Stopping behavior")
                .WithoutException();
    }
}
