using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

namespace Modix.Business.Guilds.Tracking
{
    internal static partial class GuildTrackingLogMessages
    {
        public static void GuildTracked(
                ILogger                 logger,
                GuildTrackingCacheEntry entry)
            => GuildTracked(
                logger,
                entry.Id,
                entry.Name,
                entry.Icon?.Value);

        [LoggerMessage(
            EventId = 0x08B995B6,
            Level   = LogLevel.Debug,
            Message = "Guild tracked (GuildId {GuildId}, {Name})")]
        private static partial void GuildTracked(
                ILogger                 logger,
                Snowflake   guildId,
                string      name,
                string?     iconHash);

        public static void GuildTracking(
                ILogger                 logger,
                GuildTrackingCacheEntry entry)
            => GuildTracking(
                logger,
                entry.Id,
                entry.Name,
                entry.Icon?.Value);

        [LoggerMessage(
            EventId = 0x196AFDBB,
            Level   = LogLevel.Debug,
            Message = "Tracking guild (GuildId {GuildId}, {Name})")]
        private static partial void GuildTracking(
            ILogger     logger,
            Snowflake   guildId,
            string      name,
            string?     iconHash);

        [LoggerMessage(
            EventId = 0x09BFE7A0,
            Level   = LogLevel.Debug,
            Message = "Guild un-tracked (GuildId {GuildId})")]
        public static partial void GuildUnTracked(
            ILogger     logger,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x7408806D,
            Level   = LogLevel.Debug,
            Message = "Un-tracking guild (GuildId {GuildId})")]
        public static partial void GuildUnTracking(
            ILogger     logger,
            Snowflake   guildId);
    }
}
