using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.Core;

namespace Modix.Data.Auditing
{
    internal static class AuditingLogMessages
    {
        private enum EventType
        {
            ActionCreating  = DataLogEventType.Auditing + 0x0100,
            ActionInserting = DataLogEventType.Auditing + 0x0200,
            ActionSaving    = DataLogEventType.Auditing + 0x0300,
            ActionCreated   = DataLogEventType.Auditing + 0x0400
        }

        public static void ActionCreated<TActionType>(
                    ILogger         logger,
                    long            actionId,
                    TActionType     typeId,
                    DateTimeOffset  performed,
                    Snowflake?      performedById)
                where TActionType : struct
            => _actionCreated.Invoke(
                logger,
                actionId,
                typeId.ToString()!,
                performed,
                performedById);
        private static readonly Action<ILogger, long, string, DateTimeOffset, Snowflake?> _actionCreated
            = StructuredLoggerMessage.Define<long, string, DateTimeOffset, Snowflake?>(
                    LogLevel.Information,
                    EventType.ActionCreated.ToEventId(),
                    "Audited action created (Id {ActionId}, {Type})",
                    "Performed",
                    "PerformedBy")
                .WithoutException();

        public static void ActionCreating<TActionType>(
                    ILogger         logger,
                    TActionType     typeId,
                    DateTimeOffset  performed,
                    Snowflake?      performedById)
                where TActionType : struct
            => _actionCreating.Invoke(
                logger,
                typeId.ToString()!,
                performed,
                performedById);
        private static readonly Action<ILogger, string, DateTimeOffset, Snowflake?> _actionCreating
            = StructuredLoggerMessage.Define<string, DateTimeOffset, Snowflake?>(
                    LogLevel.Debug,
                    EventType.ActionCreating.ToEventId(),
                    "Creating audited action ({ActionType})",
                    "Performed",
                    "PerformedBy")
                .WithoutException();

        public static void ActionInserting(ILogger logger)
            => _actionInserting.Invoke(logger);
        private static readonly Action<ILogger> _actionInserting
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.ActionCreating.ToEventId(),
                    "Inserting audited action")
                .WithoutException();

        public static void ActionSaving(ILogger logger)
            => _actionSaving.Invoke(logger);
        private static readonly Action<ILogger> _actionSaving
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.ActionSaving.ToEventId(),
                    "Saving audited action")
                .WithoutException();
    }
}
