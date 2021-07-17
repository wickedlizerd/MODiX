using System;

using Microsoft.Extensions.Logging;

namespace Modix.Data.Migrations
{
    internal static class MigrationsLogMessages
    {
        private enum EventType
        {
            DatabaseMigrating   = DataLogEventType.Migrations + 0x0100,
            DatabaseMigrated    = DataLogEventType.Migrations + 0x0200
        }

        public static void DatabaseMigrated(ILogger logger)
            => _databaseMigrated.Invoke(logger);
        private static readonly Action<ILogger> _databaseMigrated
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.DatabaseMigrated.ToEventId(),
                    "Database migrations applied")
                .WithoutException();

        public static void DatabaseMigrating(ILogger logger)
            => _databaseMigrating.Invoke(logger);
        private static readonly Action<ILogger> _databaseMigrating
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.DatabaseMigrating.ToEventId(),
                    "Applying database migrations")
                .WithoutException();
    }
}
