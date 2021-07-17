using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Modix.Common.ObjectModel;

using Snowflake = Remora.Discord.Core.Snowflake;

namespace Modix.Business.Users.Tracking
{
    internal static class UserTrackingLogMessages
    {
        private enum EventType
        {
            CacheEntryRetrieving            = 0x01,
            CacheEntryNotFound              = 0x02,
            CacheEntryRetrieved             = 0x03,
            CacheEntryReplacing             = 0x04,
            CacheEntryReplaced              = 0x05,
            CacheEntriesRemoving            = 0x06,
            CacheEntriesRemoved             = 0x07,
            CacheCleaning                   = 0x08,
            CacheCleaned                    = 0x09,
            CacheEntriesResetNotNeeded      = 0x0A,
            CacheEntriesResetting           = 0x0B,
            CacheEntriesReset               = 0x0C,
            CacheEntriesSaveNotNeeded       = 0x0D,
            CacheEntriesSaving              = 0x0E,
            CacheEntriesSaved               = 0x0F,
            CacheEntryAdding                = 0x10,
            CacheEntryAdded                 = 0x11,
            CacheEntryResetting             = 0x12,
            CacheEntryReset                 = 0x13,
            CacheEntrySaving                = 0x14,
            CacheEntrySaved                 = 0x15,
            UserTracking                    = 0x16,
            UserTracked                     = 0x17
        }

        public static void CacheCleaned(ILogger logger)
            => _cacheCleaned.Invoke(logger);
        private static readonly Action<ILogger> _cacheCleaned
            = LoggerMessage.Define(
                    LogLevel.Information,
                    EventType.CacheCleaned.ToEventId(),
                    "Cache cleaned")
                .WithoutException();

        public static void CacheCleaning(ILogger logger)
            => _cacheCleaning.Invoke(logger);
        private static readonly Action<ILogger> _cacheCleaning
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.CacheCleaning.ToEventId(),
                    "Cleaning cache")
                .WithoutException();

        public static void CacheEntriesRemoved(ILogger logger)
            => _cacheEntriesRemoved.Invoke(logger);
        private static readonly Action<ILogger> _cacheEntriesRemoved
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.CacheEntriesRemoved.ToEventId(),
                    "Cache entries removed")
                .WithoutException();

        public static void CacheEntriesRemoving(
                ILogger     logger,
                TimeSpan    minimumAge)
            => _cacheEntriesRemoving.Invoke(
                logger,
                minimumAge);
        private static readonly Action<ILogger, TimeSpan> _cacheEntriesRemoving
            = LoggerMessage.Define<TimeSpan>(
                    LogLevel.Debug,
                    EventType.CacheEntriesRemoving.ToEventId(),
                    "Removing cache entries (older than {MinimumAge})")
                .WithoutException();

        public static void CacheEntriesReset(
                ILogger logger,
                int     entryCount)
            => _cacheEntriesReset.Invoke(
                logger,
                entryCount);
        private static readonly Action<ILogger, int> _cacheEntriesReset
            = LoggerMessage.Define<int>(
                    LogLevel.Debug,
                    EventType.CacheEntriesReset.ToEventId(),
                    "Cache entries reset ({EntryCount} entries)")
                .WithoutException();

        public static void CacheEntriesResetNotNeeded(ILogger logger)
            => _cacheEntriesResetNotNeeded.Invoke(logger);
        private static readonly Action<ILogger> _cacheEntriesResetNotNeeded
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.CacheEntriesResetNotNeeded.ToEventId(),
                    "No cache entries need reset")
                .WithoutException();

        public static void CacheEntriesResetting(
                ILogger logger,
                int     entryCount)
            => _cacheEntriesResetting.Invoke(
                logger,
                entryCount);
        private static readonly Action<ILogger, int> _cacheEntriesResetting
            = LoggerMessage.Define<int>(
                    LogLevel.Debug,
                    EventType.CacheEntriesResetting.ToEventId(),
                    "Resetting cache entries ({EntryCount} entries)")
                .WithoutException();

        public static void CacheEntriesSaved(
                ILogger logger,
                int     entryCount)
            => _cacheEntriesSaved.Invoke(
                logger,
                entryCount);
        private static readonly Action<ILogger, int> _cacheEntriesSaved
            = LoggerMessage.Define<int>(
                    LogLevel.Debug,
                    EventType.CacheEntriesSaved.ToEventId(),
                    "Cache entries saved ({EntryCount} entries)")
                .WithoutException();

        public static void CacheEntriesSaving(
                ILogger logger,
                int     entryCount)
            => _cacheEntriesSaving.Invoke(
                logger,
                entryCount);
        private static readonly Action<ILogger, int> _cacheEntriesSaving
            = LoggerMessage.Define<int>(
                    LogLevel.Debug,
                    EventType.CacheEntriesSaving.ToEventId(),
                    "Saving cache entries ({EntryCount} entries)")
                .WithoutException();

        public static void CacheEntriesSaveNotNeeded(ILogger logger)
            => _cacheEntriesSaveNotNeeded.Invoke(logger);
        private static readonly Action<ILogger> _cacheEntriesSaveNotNeeded
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.CacheEntriesSaveNotNeeded.ToEventId(),
                    "No cache entries need saved")
                .WithoutException();

        public static void CacheEntryNotFound(
                ILogger     logger,
                Snowflake   userId)
            => _cacheEntryNotFound.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _cacheEntryNotFound
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.CacheEntryNotFound.ToEventId(),
                    "Cache entry not found: (UserId {UserId})")
                .WithoutException();

        public static void CacheEntryAdded(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => _cacheEntryAdded.Invoke(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset, DateTimeOffset> _cacheEntryAdded
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.CacheEntryAdded.ToEventId(),
                    "Cache entry added: (UserId {UserId})",
                    "LastUpdated",
                    "LastSaved")
                .WithoutException();

        public static void CacheEntryAdding(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => _cacheEntryAdding.Invoke(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset, DateTimeOffset> _cacheEntryAdding
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.CacheEntryAdding.ToEventId(),
                    "Adding cache entry: (UserId {UserId})",
                    "LastUpdated",
                    "LastSaved")
                .WithoutException();

        public static void CacheEntryReplaced(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => _cacheEntryReplaced.Invoke(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset, DateTimeOffset> _cacheEntryReplaced
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.CacheEntryReplaced.ToEventId(),
                    "Cache entry replaced: (UserId {UserId})",
                    "LastUpdated",
                    "LastSaved")
                .WithoutException();

        public static void CacheEntryReplacing(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => _cacheEntryReplacing.Invoke(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset, DateTimeOffset> _cacheEntryReplacing
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.CacheEntryReplacing.ToEventId(),
                    "Replacing cache entry: (UserId {UserId})",
                    "LastUpdated",
                    "LastSaved")
                .WithoutException();

        public static void CacheEntryReset(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => _cacheEntryReset.Invoke(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset, DateTimeOffset> _cacheEntryReset
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.CacheEntryReset.ToEventId(),
                    "Cache entry reset: (UserId {UserId})",
                    "LastUpdated",
                    "LastSaved")
                .WithoutException();

        public static void CacheEntryResetting(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => _cacheEntryResetting.Invoke(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset, DateTimeOffset> _cacheEntryResetting
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.CacheEntryResetting.ToEventId(),
                    "Resetting cache entry: (UserId {UserId})",
                    "LastUpdated",
                    "LastSaved")
                .WithoutException();

        public static void CacheEntryRetrieved(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => _cacheEntryRetrieved.Invoke(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset, DateTimeOffset> _cacheEntryRetrieved
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.CacheEntryRetrieved.ToEventId(),
                    "Cache entry retrieved: (UserId {UserId})",
                    "LastUpdated",
                    "LastSaved")
                .WithoutException();

        public static void CacheEntryRetrieving(
                ILogger     logger,
                Snowflake   userId)
            => _cacheEntryRetrieving.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _cacheEntryRetrieving
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.CacheEntryRetrieving.ToEventId(),
                    "Retrieving cache entry: (UserId {UserId})")
                .WithoutException();

        public static void CacheEntrySaved(
                ILogger     logger,
                Snowflake   userId,
                Snowflake   guildId)
            => _cacheEntrySaved.Invoke(
                logger,
                userId,
                guildId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _cacheEntrySaved
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.CacheEntrySaved.ToEventId(),
                    "Cache entry saved: (UserId {UserId}, GuildId {GuildId})")
                .WithoutException();

        public static void CacheEntrySaving(
                ILogger     logger,
                Snowflake   userId,
                Snowflake   guildId)
            => _cacheEntrySaving.Invoke(
                logger,
                userId,
                guildId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _cacheEntrySaving
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.CacheEntrySaving.ToEventId(),
                    "Saving cache entry: (UserId {UserId}, GuildId {GuildId})")
                .WithoutException();

        public static void UserTracked(
                ILogger     logger,
                Snowflake   userId,
                Snowflake   guildId)
            => _userTracked.Invoke(
                logger,
                userId,
                guildId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _userTracked
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.UserTracked.ToEventId(),
                    "User tracked: UserId {UserId}, GuildId {GuildId}")
                .WithoutException();

        public static void UserTracking(
                ILogger             logger,
                Snowflake           userId,
                Snowflake           guildId,
                Optional<string>    username,
                Optional<ushort>    discriminator,
                Optional<string?>   avatarHash,
                Optional<string?>   nickname)
            => _userTracking.Invoke(
                logger,
                userId,
                guildId,
                username.ToString(),
                discriminator.ToString(),
                avatarHash.ToString(),
                nickname.ToString());
        private static readonly Action<ILogger, Snowflake, Snowflake, string, string, string, string> _userTracking
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, string, string, string, string>(
                    LogLevel.Debug,
                    EventType.UserTracking.ToEventId(),
                    "Tracking user: UserId {UserId}, GuildId {GuildId}",
                    "Username",
                    "Discriminator",
                    "AvatarHash",
                    "Nickname")
                .WithoutException();
    }
}
