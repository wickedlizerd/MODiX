using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.Core;

namespace Modix.Business.Users.Tracking
{
    internal static partial class UserTrackingLogMessages
    {
        public static IDisposable BeginBackgroundScope(
                ILogger logger,
                Guid    guid)
            => _beginBackgroundScope.Invoke(
                logger,
                guid);
        private static readonly Func<ILogger, Guid, IDisposable> _beginBackgroundScope
            = StructuredLoggerMessage.DefineScopeData<Guid>("ScopeId");

        [LoggerMessage(
            EventId = 0x2E5EE16B,
            Level   = LogLevel.Information,
            Message = "Cache cleaned")]
        public static partial void CacheCleaned(ILogger logger);

        [LoggerMessage(
            EventId = 0x2EB87B70,
            Level   = LogLevel.Debug,
            Message = "Cleaning cache")]
        public static partial void CacheCleaning(ILogger logger);

        [LoggerMessage(
            EventId = 0x5AC9DD13,
            Level   = LogLevel.Debug,
            Message = "Cache entries removed")]
        public static partial void CacheEntriesRemoved(ILogger logger);

        [LoggerMessage(
            EventId = 0x3A11BD7F,
            Level   = LogLevel.Debug,
            Message = "Removing cache entries (older than {MinimumAge})")]
        public static partial void CacheEntriesRemoving(
            ILogger     logger,
            TimeSpan    minimumAge);

        [LoggerMessage(
            EventId = 0x3B8811EE,
            Level   = LogLevel.Debug,
            Message = "Cache entries reset ({EntryCount} entries)")]
        public static partial void CacheEntriesReset(
            ILogger logger,
            int     entryCount);

        [LoggerMessage(
            EventId = 0x6A80D516,
            Level   = LogLevel.Debug,
            Message = "No cache entries need reset")]
        public static partial void CacheEntriesResetNotNeeded(ILogger logger);

        [LoggerMessage(
            EventId = 0x3BEA7AE1,
            Level   = LogLevel.Debug,
            Message = "Resetting cache entries ({EntryCount} entries)")]
        public static partial void CacheEntriesResetting(
            ILogger logger,
            int     entryCount);

        [LoggerMessage(
            EventId = 0x640B87DC,
            Level   = LogLevel.Debug,
            Message = "Cache entries saved ({EntryCount} entries)")]
        public static partial void CacheEntriesSaved(
            ILogger logger,
            int     entryCount);

        [LoggerMessage(
            EventId = 0x4456B5E1,
            Level   = LogLevel.Debug,
            Message = "Saving cache entries ({EntryCount} entries)")]
        public static partial void CacheEntriesSaving(
            ILogger logger,
            int     entryCount);

        [LoggerMessage(
            EventId = 0x4110E122,
            Level   = LogLevel.Debug,
            Message = "No cache entries need saved")]
        public static partial void CacheEntriesSaveNotNeeded(ILogger logger);

        [LoggerMessage(
            EventId = 0x1365C7F4,
            Level   = LogLevel.Debug,
            Message = "Cache entry not found: (UserId {UserId})")]
        public static partial void CacheEntryNotFound(
            ILogger     logger,
            Snowflake   userId);

        public static void CacheEntryAdded(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => CacheEntryAdded(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);

        [LoggerMessage(
            EventId = 0x1AC933C0,
            Level   = LogLevel.Debug,
            Message = "Cache entry added: (UserId {UserId})")]
        private static partial void CacheEntryAdded(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  lastUpdated,
            DateTimeOffset  lastSaved);

        public static void CacheEntryAdding(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => CacheEntryAdding(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);

        [LoggerMessage(
            EventId = 0x0DDCB054,
            Level   = LogLevel.Debug,
            Message = "Adding cache entry: (UserId {UserId})")]
        private static partial void CacheEntryAdding(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  lastUpdated,
            DateTimeOffset  lastSaved);

        public static void CacheEntryReplaced(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => CacheEntryReplaced(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);

        [LoggerMessage(
            EventId = 0x4022571C,
            Level   = LogLevel.Debug,
            Message = "Cache entry replaced: (UserId {UserId})")]
        private static partial void CacheEntryReplaced(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  lastUpdated,
            DateTimeOffset  lastSaved);

        public static void CacheEntryReplacing(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => CacheEntryReplacing(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);

        [LoggerMessage(
            EventId = 0x2004A6D2,
            Level   = LogLevel.Debug,
            Message = "Replacing cache entry: (UserId {UserId})")]
        private static partial void CacheEntryReplacing(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  lastUpdated,
            DateTimeOffset  lastSaved);

        public static void CacheEntryReset(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => CacheEntryReset(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);

        [LoggerMessage(
            EventId = 0x70148F08,
            Level   = LogLevel.Debug,
            Message = "Cache entry reset: (UserId {UserId})")]
        private static partial void CacheEntryReset(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  lastUpdated,
            DateTimeOffset  lastSaved);

        public static void CacheEntryResetting(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => CacheEntryResetting(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);

        [LoggerMessage(
            EventId = 0x410D64E7,
            Level   = LogLevel.Debug,
            Message = "Resetting cache entry: (UserId {UserId})")]
        private static partial void CacheEntryResetting(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  lastUpdated,
            DateTimeOffset  lastSaved);

        public static void CacheEntryRetrieved(
                ILogger                 logger,
                UserTrackingCacheEntry  entry)
            => CacheEntryRetrieved(
                logger,
                entry.UserId,
                entry.LastUpdated,
                entry.LastSaved);

        [LoggerMessage(
            EventId = 0x54D07E2C,
            Level   = LogLevel.Debug,
            Message = "Cache entry retrieved: (UserId {UserId})")]
        private static partial void CacheEntryRetrieved(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  lastUpdated,
            DateTimeOffset  lastSaved);

        [LoggerMessage(
            EventId = 0x299DF3FF,
            Level   = LogLevel.Debug,
            Message = "Retrieving cache entry: (UserId {UserId})")]
        public static partial void CacheEntryRetrieving(
            ILogger     logger,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x7AD8B91C,
            Level   = LogLevel.Debug,
            Message = "Cache entry saved: (UserId {UserId}, GuildId {GuildId})")]
        public static partial void CacheEntrySaved(
            ILogger     logger,
            Snowflake   userId,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x7EE0190F,
            Level   = LogLevel.Debug,
            Message = "Saving cache entry: (UserId {UserId}, GuildId {GuildId})")]
        public static partial void CacheEntrySaving(
            ILogger     logger,
            Snowflake   userId,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x32FFFF5D,
            Level   = LogLevel.Debug,
            Message = "User tracked: UserId {UserId}, GuildId {GuildId}")]
        public static partial void UserTracked(
            ILogger     logger,
            Snowflake   userId,
            Snowflake   guildId);

        public static void UserTracking(
                ILogger             logger,
                Snowflake           userId,
                Snowflake           guildId,
                Optional<string>    username,
                Optional<ushort>    discriminator,
                Optional<string?>   avatarHash,
                Optional<string?>   nickname)
            => UserTracking(
                logger,
                userId,
                guildId,
                username.ToString(),
                discriminator.ToString(),
                avatarHash.ToString(),
                nickname.ToString());

        [LoggerMessage(
            EventId = 0x5E4CE7E9,
            Level   = LogLevel.Debug,
            Message = "Tracking user: UserId {UserId}, GuildId {GuildId}")]
        private static partial void UserTracking(
            ILogger     logger,
            Snowflake   userId,
            Snowflake   guildId,
            string      username,
            string      discriminator,
            string      avatarHash,
            string      nickname);
    }
}
