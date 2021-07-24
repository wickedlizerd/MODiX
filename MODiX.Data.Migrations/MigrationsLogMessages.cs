using Microsoft.Extensions.Logging;

namespace Modix.Data.Migrations
{
    internal static partial class MigrationsLogMessages
    {
        [LoggerMessage(
            EventId = 0x3C99957B,
            Level   = LogLevel.Debug,
            Message = "Database migrations applied")]
        public static partial void DatabaseMigrated(ILogger logger);

        [LoggerMessage(
            EventId = 0x7AA598B8,
            Level   = LogLevel.Debug,
            Message = "Applying database migrations")]
        public static partial void DatabaseMigrating(ILogger logger);
    }
}
