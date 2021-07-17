using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

namespace Modix.Business.Caching
{
    internal static class CachingLogMessages
    {
        private enum EventType
        {
            LockAcquiring       = 0x0001,
            LockAcquired        = 0x0002,
            EntryAdding         = 0x0003,
            EntryAdded          = 0x0004,
            EntryRetrieving     = 0x0005,
            EntryNotFound       = 0x0006,
            EntryRetrieved      = 0x0007,
            EntryReplacing      = 0x0008,
            EntryReplaced       = 0x0009,
            EntryRemoving       = 0x000A,
            EntryRemoved        = 0x000B,
            OldEntriesRemoving  = 0x000C,
            OldEntriesRemoved   = 0x000D,
            OldestEntryChanging = 0x000E,
            OldestEntryChanged  = 0x000F
        }

        public static void EntryAdded<TKey>(
                    ILogger logger,
                    TKey key)
                where TKey : notnull
            => _entryAdded1.Invoke(
                logger,
                key.ToString()!);
        private static readonly Action<ILogger, string> _entryAdded1
            = LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    EventType.EntryAdded.ToEventId(),
                    "Entry added: (Key {Key})")
                .WithoutException();

        public static void EntryAdded<TKey>(
                    ILogger             logger,
                    TKey                key,
                    DateTimeOffset      added)
                where TKey : notnull
            => _entryAdded2.Invoke(
                logger,
                key.ToString(),
                added);
        private static readonly Action<ILogger, string?, DateTimeOffset> _entryAdded2
            = StructuredLoggerMessage.Define<string?, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.EntryAdded.ToEventId(),
                    "Entry added: (Key {Key})",
                    "Added")
                .WithoutException();

        public static void EntryAdding<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => _entryAdding.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryAdding
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryAdding.ToEventId(),
                    "Adding entry: (Key {Key})")
                .WithoutException();

        public static void EntryNotFound<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => _entryNotFound.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryNotFound
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryNotFound.ToEventId(),
                    "Entry not found: (Key {Key})")
                .WithoutException();

        public static void EntryRemoved<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => _entryRemoved1.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryRemoved1
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryRemoved.ToEventId(),
                    "Entry removed: (Key {Key})")
                .WithoutException();

        public static void EntryRemoved<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => _entryRemoved2.Invoke(
                logger,
                key.ToString(),
                added);
        private static readonly Action<ILogger, string?, DateTimeOffset> _entryRemoved2
            = StructuredLoggerMessage.Define<string?, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.EntryRemoved.ToEventId(),
                    "Entry removed: (Key {Key})",
                    "Added")
                .WithoutException();

        public static void EntryRemoving<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => _entryRemoving1.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryRemoving1
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryRemoving.ToEventId(),
                    "Removing entry: (Key {Key})")
                .WithoutException();

        public static void EntryRemoving<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => _entryRemoving2.Invoke(
                logger,
                key.ToString(),
                added);
        private static readonly Action<ILogger, string?, DateTimeOffset> _entryRemoving2
            = StructuredLoggerMessage.Define<string?, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.EntryRemoving.ToEventId(),
                    "Removing entry: (Key {Key})",
                    "Added")
                .WithoutException();

        public static void EntryReplaced<TKey>(
                    ILogger             logger,
                    TKey                key)
                where TKey : notnull
            => _entryReplaced.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryReplaced
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryReplaced.ToEventId(),
                    "Entry replaced: (Key {Key})")
                .WithoutException();

        public static void EntryReplaced<TKey>(
                    ILogger             logger,
                    TKey                key,
                    DateTimeOffset      added)
                where TKey : notnull
            => _entryReplaced2.Invoke(
                logger,
                key.ToString(),
                added);
        private static readonly Action<ILogger, string?, DateTimeOffset> _entryReplaced2
            = StructuredLoggerMessage.Define<string?, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.EntryReplaced.ToEventId(),
                    "Entry replaced: (Key {Key})",
                    "Added")
                .WithoutException();

        public static void EntryReplacing<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => _entryReplacing1.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryReplacing1
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryReplacing.ToEventId(),
                    "Replacing entry: (Key {Key})")
                .WithoutException();

        public static void EntryReplacing<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => _entryReplacing2.Invoke(
                logger,
                key.ToString(),
                added);
        private static readonly Action<ILogger, string?, DateTimeOffset> _entryReplacing2
            = StructuredLoggerMessage.Define<string?, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.EntryReplacing.ToEventId(),
                    "Replacing entry: (Key {Key})",
                    "Added")
                .WithoutException();

        public static void EntryRetrieved<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => _entryRetrieved1.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryRetrieved1
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryRetrieved.ToEventId(),
                    "Entry retrieved: (UserId {UserId})")
                .WithoutException();

        public static void EntryRetrieved<TKey>(
                    ILogger         logger,
                    TKey            key,
                    DateTimeOffset  added)
                where TKey : notnull
            => _entryRetrieved2.Invoke(
                logger,
                key.ToString(),
                added);
        private static readonly Action<ILogger, string?, DateTimeOffset> _entryRetrieved2
            = StructuredLoggerMessage.Define<string?, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.EntryRetrieved.ToEventId(),
                    "Entry retrieved: (Key {Key})",
                    "Added")
                .WithoutException();

        public static void EntryRetrieving<TKey>(
                    ILogger logger,
                    TKey    key)
                where TKey : notnull
            => _entryRetrieving.Invoke(
                logger,
                key.ToString());
        private static readonly Action<ILogger, string?> _entryRetrieving
            = LoggerMessage.Define<string?>(
                    LogLevel.Debug,
                    EventType.EntryRetrieving.ToEventId(),
                    "Retrieving entry: (UserId {UserId})")
                .WithoutException();

        public static void LockAcquired(ILogger logger)
            => _lockAcquired.Invoke(logger);
        private static readonly Action<ILogger> _lockAcquired
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.LockAcquired.ToEventId(),
                    "Lock acquired")
                .WithoutException();

        public static void LockAcquiring(ILogger logger)
            => _lockAcquiring.Invoke(logger);
        private static readonly Action<ILogger> _lockAcquiring
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.LockAcquiring.ToEventId(),
                    "Acquiring lock")
                .WithoutException();

        public static void OldEntriesRemoved(ILogger logger)
            => _oldEntriesRemoved.Invoke(logger);
        private static readonly Action<ILogger> _oldEntriesRemoved
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.OldEntriesRemoved.ToEventId(),
                    "Old entries removed")
                .WithoutException();

        public static void OldEntriesRemoving(
                ILogger     logger,
                TimeSpan    minimumAge)
            => _oldEntriesRemoving.Invoke(
                logger,
                minimumAge);
        private static readonly Action<ILogger, TimeSpan> _oldEntriesRemoving
            = LoggerMessage.Define<TimeSpan>(
                    LogLevel.Debug,
                    EventType.OldEntriesRemoving.ToEventId(),
                    "Removing old entries (older than {MinimumAge})")
                .WithoutException();

        public static void OldestEntryChanged(
                ILogger         logger,
                DateTimeOffset? oldestEntryAdded)
            => _oldestEntryChanged.Invoke(
                logger,
                oldestEntryAdded);
        private static readonly Action<ILogger, DateTimeOffset?> _oldestEntryChanged
            = LoggerMessage.Define<DateTimeOffset?>(
                    LogLevel.Debug,
                    EventType.OldestEntryChanged.ToEventId(),
                    "Oldest entry changed: (Added {OldestEntryAdded})")
                .WithoutException();

        public static void OldestEntryChanging(
                ILogger         logger,
                DateTimeOffset? oldestEntryAdded)
            => _oldestEntryChanging.Invoke(
                logger,
                oldestEntryAdded);
        private static readonly Action<ILogger, DateTimeOffset?> _oldestEntryChanging
            = LoggerMessage.Define<DateTimeOffset?>(
                    LogLevel.Debug,
                    EventType.OldestEntryChanged.ToEventId(),
                    "Changing oldest entry: (Added {OldestEntryAdded})")
                .WithoutException();
    }
}
