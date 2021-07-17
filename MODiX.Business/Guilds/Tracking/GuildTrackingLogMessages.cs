using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.Core;

namespace Modix.Business.Guilds.Tracking
{
    internal static class GuildTrackingLogMessages
    {
        private enum EventType
        {
            GuildTracking   = GuildsLogEventType.Tracking + 0x01,
            GuildTracked    = GuildsLogEventType.Tracking + 0x02,
            GuildUnTracking = GuildsLogEventType.Tracking + 0x03,
            GuildUnTracked  = GuildsLogEventType.Tracking + 0x04
        }

        public static void GuildTracked(
                ILogger                 logger,
                GuildTrackingCacheEntry entry)
            => _guildTracked.Invoke(
                logger,
                entry.Id,
                entry.Name,
                entry.Icon?.Value);
        private static readonly Action<ILogger, Snowflake, string, string?> _guildTracked
            = StructuredLoggerMessage.Define<Snowflake, string, string?>(
                    LogLevel.Debug,
                    EventType.GuildTracked.ToEventId(),
                    "Guild tracked (GuildId {GuildId}, {Name})",
                    "IconHash")
                .WithoutException();

        public static void GuildTracking(
                ILogger                 logger,
                GuildTrackingCacheEntry entry)
            => _guildTracking.Invoke(
                logger,
                entry.Id,
                entry.Name,
                entry.Icon?.Value);
        private static readonly Action<ILogger, Snowflake, string, string?> _guildTracking
            = StructuredLoggerMessage.Define<Snowflake, string, string?>(
                    LogLevel.Debug,
                    EventType.GuildTracking.ToEventId(),
                    "Tracking guild (GuildId {GuildId}, {Name})",
                    "IconHash")
                .WithoutException();

        public static void GuildUnTracked(
                ILogger     logger,
                Snowflake   guildId)
            => _guildUnTracked.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _guildUnTracked
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildUnTracked.ToEventId(),
                    "Guild un-tracked (GuildId {GuildId})")
                .WithoutException();

        public static void GuildUnTracking(
                ILogger     logger,
                Snowflake   guildId)
            => _guildUnTracking.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _guildUnTracking
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildUnTracking.ToEventId(),
                    "Un-tracking guild (GuildId {GuildId})")
                .WithoutException();
    }
}
