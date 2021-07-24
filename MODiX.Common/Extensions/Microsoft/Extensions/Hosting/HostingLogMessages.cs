using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    internal static partial class HostingLogMessages
    {
        [LoggerMessage(
            EventId = 0x6063D5CC,
            Level   = LogLevel.Information,
            Message = "Startup action executed")]
        public static partial void StartupActionExecuted(ILogger logger);

        [LoggerMessage(
            EventId = 0x15E90E2F,
            Level   = LogLevel.Information,
            Message = "Executing startup action")]
        public static partial void StartupActionExecuting(ILogger logger);
    }
}
