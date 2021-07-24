using System;

using Microsoft.Extensions.Logging;

namespace Modix.Business.Caching
{
    internal static partial class CachingLogMessages
    {
        public static void EntryAdded<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryAdded_Internal(
                logger,
                key.ToString());

        public static void EntryAdded<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => EntryAdded_Internal(
                logger,
                key.ToString(),
                added);

        [LoggerMessage(
            EventId     = 0x1E323699,
            EventName   = nameof(EntryAdded),
            Level       = LogLevel.Debug,
            Message     = "Entry added: (Key {Key})")]
        private static partial void EntryAdded_Internal(
            ILogger         logger,
            string?         key,
            DateTimeOffset? added = default);

        public static void EntryAdding<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryAdding_Internal(
                logger,
                key.ToString());

        [LoggerMessage(
            EventId     = 0x2D62CBE9,
            EventName   = nameof(EntryAdding),
            Level       = LogLevel.Debug,
            Message     = "Adding entry: (Key {Key})")]
        private static partial void EntryAdding_Internal(
            ILogger logger,
            string? key);

        public static void EntryNotFound<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryNotFound_Internal(
                logger,
                key.ToString());

        [LoggerMessage(
            EventId     = 0x0B1C8BF2,
            EventName   = nameof(EntryNotFound),
            Level       = LogLevel.Debug,
            Message     = "Entry not found: (Key {Key})")]
        private static partial void EntryNotFound_Internal(
            ILogger logger,
            string? key);

        public static void EntryRemoved<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryRemoved_Internal(
                logger,
                key.ToString());

        public static void EntryRemoved<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => EntryRemoved_Internal(
                logger,
                key.ToString(),
                added);

        [LoggerMessage(
            EventId     = 0x230F8BBA,
            EventName   = nameof(EntryRemoved),
            Level       = LogLevel.Debug,
            Message     = "Entry removed: (Key {Key})")]
        private static partial void EntryRemoved_Internal(
            ILogger         logger,
            string?         key,
            DateTimeOffset? added = default);

        public static void EntryRemoving<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryRemoving_Internal(
                logger,
                key.ToString());

        public static void EntryRemoving<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => EntryRemoving_Internal(
                logger,
                key.ToString(),
                added);

        [LoggerMessage(
            EventId     = 0x4CBD74DD,
            EventName   = nameof(EntryRemoving),
            Level       = LogLevel.Debug,
            Message     = "Removing entry: (Key {Key})")]
        private static partial void EntryRemoving_Internal(
            ILogger         logger,
            string?         key,
            DateTimeOffset? added = default);

        public static void EntryReplaced<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryReplaced_Internal(
                logger,
                key.ToString());

        public static void EntryReplaced<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => EntryReplaced_Internal(
                logger,
                key.ToString(),
                added);

        [LoggerMessage(
            EventId     = 0x465A7E96,
            EventName   = nameof(EntryReplaced),
            Level       = LogLevel.Debug,
            Message     = "Entry replaced: (Key {Key})")]
        private static partial void EntryReplaced_Internal(
            ILogger         logger,
            string?         key,
            DateTimeOffset? added = default);

        public static void EntryReplacing<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryReplacing_Internal(
                logger,
                key.ToString());

        public static void EntryReplacing<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => EntryReplacing_Internal(
                logger,
                key.ToString(),
                added);

        [LoggerMessage(
            EventId     = 0x7839AC9C,
            EventName   = nameof(EntryReplacing),
            Level       = LogLevel.Debug,
            Message     = "Replacing entry: (Key {Key})")]
        private static partial void EntryReplacing_Internal(
            ILogger         logger,
            string?         key,
            DateTimeOffset? added = default);

        public static void EntryRetrieved<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryRetrieved_Internal(
                logger,
                key.ToString());

        public static void EntryRetrieved<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => EntryRetrieved_Internal(
                logger,
                key.ToString(),
                added);

        [LoggerMessage(
            EventId     = 0x497EA4B6,
            EventName   = nameof(EntryRetrieved),
            Level       = LogLevel.Debug,
            Message     = "Entry retrieved: (Key {Key})")]
        private static partial void EntryRetrieved_Internal(
            ILogger         logger,
            string?         key,
            DateTimeOffset? added = default);

        public static void EntryRetrieving<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => EntryRetrieving_Internal(
                logger,
                key.ToString());

        [LoggerMessage(
            EventId     = 0x7F8E83BD,
            EventName   = nameof(EntryRetrieving),
            Level       = LogLevel.Debug,
            Message     = "Retrieving entry: (Key {Key})")]
        public static partial void EntryRetrieving_Internal(
            ILogger logger,
            string? key);

        [LoggerMessage(
            EventId = 0x4206E0AC,
            Level   = LogLevel.Debug,
            Message = "Lock acquired")]
        public static partial void LockAcquired(ILogger logger);

        [LoggerMessage(
            EventId = 0x6342B162,
            Level   = LogLevel.Debug,
            Message = "Acquiring lock")]
        public static partial void LockAcquiring(ILogger logger);

        [LoggerMessage(
            EventId = 0x383ED0CB,
            Level   = LogLevel.Debug,
            Message = "Old entries removed")]
        public static partial void OldEntriesRemoved(ILogger logger);

        [LoggerMessage(
            EventId = 0x0B8CCC78,
            Level   = LogLevel.Debug,
            Message = "Removing old entries (older than {MinimumAge})")]
        public static partial void OldEntriesRemoving(
            ILogger     logger,
            TimeSpan    minimumAge);

        [LoggerMessage(
            EventId = 0x2E6EAB09,
            Level   = LogLevel.Debug,
            Message = "Oldest entry changed: (Added {OldestEntryAdded})")]
        public static partial void OldestEntryChanged(
            ILogger         logger,
            DateTimeOffset? oldestEntryAdded);

        [LoggerMessage(
            EventId = 0x6A801BF6,
            Level   = LogLevel.Debug,
            Message = "Changing oldest entry: (Added {OldestEntryAdded})")]
        public static partial void OldestEntryChanging(
            ILogger         logger,
            DateTimeOffset? oldestEntryAdded);
    }
}
