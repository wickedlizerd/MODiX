using System;

using Microsoft.Extensions.Logging;

using Modix.Common;

namespace Microsoft.Extensions.Hosting
{
    internal static class HostingLogMessages
    {
        private enum EventType
        {
            StartupActionExecuting  = CommonLogEventType.Hosting + 0x0001,
            StartupActionExecuted   = CommonLogEventType.Hosting + 0x0002,
        }

        public static void StartupActionExecuted(ILogger logger)
            => _startupActionExecuted.Invoke(logger);
        private static readonly Action<ILogger> _startupActionExecuted
            = LoggerMessage.Define(
                    LogLevel.Information,
                    EventType.StartupActionExecuted.ToEventId(),
                    $"{nameof(StartupActionBase)} executed")
                .WithoutException();

        public static void StartupActionExecuting(ILogger logger)
            => _startupActionExecuting.Invoke(logger);
        private static readonly Action<ILogger> _startupActionExecuting
            = LoggerMessage.Define(
                    LogLevel.Information,
                    EventType.StartupActionExecuting.ToEventId(),
                    $"Executing {nameof(StartupActionBase)}")
                .WithoutException();
    }
}
