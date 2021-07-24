using System;

using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

namespace Modix.Data.Auditing
{
    internal static partial class AuditingLogMessages
    {
        public static void ActionCreated<TActionType>(
                    ILogger         logger,
                    long            actionId,
                    TActionType     typeId,
                    DateTimeOffset  performed,
                    Snowflake?      performedById)
                where TActionType : struct
            => ActionCreated(
                logger,
                actionId,
                typeId.ToString()!,
                performed,
                performedById);

        [LoggerMessage(
            EventId = 0x24549FE8,
            Level   = LogLevel.Information,
            Message = "Audited action created (Id {ActionId}, {Type})")]
        private static partial void ActionCreated(
            ILogger         logger,
            long            actionId,
            string          type,
            DateTimeOffset  performed,
            Snowflake?      performedById);

        public static void ActionCreating<TActionType>(
                    ILogger         logger,
                    TActionType     typeId,
                    DateTimeOffset  performed,
                    Snowflake?      performedById)
                where TActionType : struct
            => ActionCreating(
                logger,
                typeId.ToString()!,
                performed,
                performedById);

        [LoggerMessage(
            EventId = 0x1401A472,
            Level   = LogLevel.Debug,
            Message = "Creating audited action ({Type})")]
        private static partial void ActionCreating(
            ILogger         logger,
            string          type,
            DateTimeOffset  performed,
            Snowflake?      performedById);

        [LoggerMessage(
            EventId = 0x5575604E,
            Level   = LogLevel.Debug,
            Message = "Inserting audited action")]
        public static partial void ActionInserting(ILogger logger);

        [LoggerMessage(
            EventId = 0x53634992,
            Level   = LogLevel.Debug,
            Message = "Saving audited action")]
        public static partial void ActionSaving(ILogger logger);
    }
}
