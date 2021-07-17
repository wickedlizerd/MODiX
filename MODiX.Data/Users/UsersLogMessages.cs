using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Modix.Common.ObjectModel;

using Snowflake = Remora.Discord.Core.Snowflake;

namespace Modix.Data.Users
{
    internal static class UsersLogMessages
    {
        private enum EventType
        {
            UserMerging                         = DataLogEventType.Users + 0x0001,
            UserMerged                          = DataLogEventType.Users + 0x0002,
            UsersMerging                        = DataLogEventType.Users + 0x0003,
            UsersMerged                         = DataLogEventType.Users + 0x0004,
            UserRetrieving                      = DataLogEventType.Users + 0x0005,
            UserRetrieved                       = DataLogEventType.Users + 0x0006,
            UserNotFound                        = DataLogEventType.Users + 0x0007,
            UserCreating                        = DataLogEventType.Users + 0x0008,
            UserCreated                         = DataLogEventType.Users + 0x0009,
            UserCurrentVersionRetrieving        = DataLogEventType.Users + 0x000A,
            UserCurrentVersionUpToDate          = DataLogEventType.Users + 0x000B,
            UserCurrentVersionOutOfDate         = DataLogEventType.Users + 0x000C,
            UserVersionCreating                 = DataLogEventType.Users + 0x000D,
            UserVersionCreated                  = DataLogEventType.Users + 0x000E,
            GuildUserRetrieving                 = DataLogEventType.Users + 0x000F,
            GuildUserRetrieved                  = DataLogEventType.Users + 0x0010,
            GuildUserNotFound                   = DataLogEventType.Users + 0x0011,
            GuildUserCreating                   = DataLogEventType.Users + 0x0012,
            GuildUserCreated                    = DataLogEventType.Users + 0x0013,
            GuildUserCurrentVersionRetrieving   = DataLogEventType.Users + 0x0014,
            GuildUserCurrentVersionUpToDate     = DataLogEventType.Users + 0x0015,
            GuildUserCurrentVersionOutOfDate    = DataLogEventType.Users + 0x0016,
            GuildUserVersionCreating            = DataLogEventType.Users + 0x0017,
            GuildUserVersionCreated             = DataLogEventType.Users + 0x0018
        }

        public static void GuildUserCreated(
                ILogger         logger,
                GuildUserEntity guildUser)
            => _guildUserCreated.Invoke(
                logger,
                guildUser.GuildId,
                guildUser.UserId,
                guildUser.FirstSeen);
        private static readonly Action<ILogger, Snowflake, Snowflake, DateTimeOffset> _guildUserCreated
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.GuildUserCreated.ToEventId(),
                    $"{nameof(GuildUserEntity)} created: GuildId {{GuildId}}, UserId {{UserId}}",
                    "FirstSeen")
                .WithoutException();

        public static void GuildUserCreating(
                ILogger         logger,
                GuildUserEntity guildUser)
            => _guildUserCreating.Invoke(
                logger,
                guildUser.GuildId,
                guildUser.UserId,
                guildUser.FirstSeen);
        private static readonly Action<ILogger, Snowflake, Snowflake, DateTimeOffset> _guildUserCreating
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.GuildUserCreating.ToEventId(),
                    $"Creating {nameof(GuildUserEntity)}: GuildId {{GuildId}}, UserId {{UserId}}",
                    "FirstSeen")
                .WithoutException();

        public static void GuildUserCurrentVersionOutOfDate(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId,
                long?       guildUserVersionId)
            => _guildUserCurrentVersionOutOfDate.Invoke(
                logger,
                guildId,
                userId,
                guildUserVersionId);
        private static readonly Action<ILogger, Snowflake, Snowflake, long?> _guildUserCurrentVersionOutOfDate
            = LoggerMessage.Define<Snowflake, Snowflake, long?>(
                    LogLevel.Debug,
                    EventType.GuildUserCurrentVersionOutOfDate.ToEventId(),
                    $"Current {nameof(GuildUserVersionEntity)} is out-of-date: GuildId {{GuildId}}, UserId {{UserId}}, GuildUserVersionId {{GuildUserVersionId}}")
                .WithoutException();

        public static void GuildUserCurrentVersionRetrieving(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId)
            => _guildUserCurrentVersionRetrieving.Invoke(
                logger,
                guildId,
                userId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _guildUserCurrentVersionRetrieving
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildUserCurrentVersionRetrieving.ToEventId(),
                    $"Retrieving current {nameof(GuildUserVersionEntity)}: GuildId {{GuildId}}, UserId {{UserID}}")
                .WithoutException();

        public static void GuildUserCurrentVersionUpToDate(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId,
                long        guildUserVersionId)
            => _guildUserCurrentVersionUpToDate.Invoke(
                logger,
                guildId,
                userId,
                guildUserVersionId);
        private static readonly Action<ILogger, Snowflake, Snowflake, long> _guildUserCurrentVersionUpToDate
            = LoggerMessage.Define<Snowflake, Snowflake, long>(
                    LogLevel.Debug,
                    EventType.GuildUserCurrentVersionUpToDate.ToEventId(),
                    $"Current {nameof(GuildUserVersionEntity)} is up-to-date: GuildId {{GuildId}}, UserId {{UserId}}, GuildUserVersionId {{GuildUserVersionId}}")
                .WithoutException();

        public static void GuildUserNotFound(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId)
            => _guildUserNotFound.Invoke(
                logger,
                guildId,
                userId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _guildUserNotFound
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildUserNotFound.ToEventId(),
                    $"{nameof(GuildUserEntity)} not found: GuildId {{GuildId}}, UserId {{UserId}}")
                .WithoutException();

        public static void GuildUserRetrieved(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId)
            => _guildUserRetrieved.Invoke(
                logger,
                guildId,
                userId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _guildUserRetrieved
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildUserRetrieved.ToEventId(),
                    $"{nameof(GuildUserEntity)} retrieved: GuildId {{GuildId}}, UserId {{UserId}}")
                .WithoutException();

        public static void GuildUserRetrieving(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId)
            => _guildUserRetrieving.Invoke(
                logger,
                guildId,
                userId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _guildUserRetrieving
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildUserRetrieving.ToEventId(),
                    $"Retrieving {nameof(GuildUserEntity)}: GuildId {{GuildId}}, UserId {{UserId}}")
                .WithoutException();

        public static void GuildUserVersionCreated(
                ILogger                 logger,
                GuildUserVersionEntity  guildUserVersion)
            => _guildUserVersionCreated.Invoke(
                logger,
                guildUserVersion.GuildId,
                guildUserVersion.UserId,
                guildUserVersion.Id,
                guildUserVersion.PreviousVersionId,
                guildUserVersion.Created,
                guildUserVersion.Nickname);
        private static readonly Action<ILogger, Snowflake, Snowflake, long, long?, DateTimeOffset, string?> _guildUserVersionCreated
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, long, long?, DateTimeOffset, string?>(
                    LogLevel.Debug,
                    EventType.GuildUserVersionCreated.ToEventId(),
                    $"{nameof(GuildUserVersionEntity)} created: GuildId {{GuildId}}, UserId {{UserId}}, GuildUserVersionId {{GuildUserVersionId}}",
                    "PreviousVersionId",
                    "Created",
                    "Nickname")
                .WithoutException();

        public static void GuildUserVersionCreating(
                ILogger                 logger,
                GuildUserVersionEntity  guildUserVersion)
            => _guildUserVersionCreating.Invoke(
                logger,
                guildUserVersion.GuildId,
                guildUserVersion.UserId,
                guildUserVersion.PreviousVersionId,
                guildUserVersion.Created,
                guildUserVersion.Nickname);
        private static readonly Action<ILogger, Snowflake, Snowflake, long?, DateTimeOffset, string?> _guildUserVersionCreating
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, long?, DateTimeOffset, string?>(
                    LogLevel.Debug,
                    EventType.GuildUserVersionCreating.ToEventId(),
                    $"Creating {nameof(GuildUserVersionEntity)}: GuildId {{GuildId}}, UserId {{UserId}}, PreviousVersionId {{PreviousVersionId}}",
                    "Created",
                    "Nickname")
                .WithoutException();

        public static void UserCreated(
                ILogger     logger,
                UserEntity  user)
            => _userCreated.Invoke(
                logger,
                user.Id,
                user.FirstSeen);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset> _userCreated
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.UserCreated.ToEventId(),
                    $"{nameof(UserEntity)} created: UserId {{UserId}}",
                    "FirstSeen")
                .WithoutException();

        public static void UserCreating(
                ILogger     logger,
                UserEntity  user)
            => _userCreating.Invoke(
                logger,
                user.Id,
                user.FirstSeen);
        private static readonly Action<ILogger, Snowflake, DateTimeOffset> _userCreating
            = StructuredLoggerMessage.Define<Snowflake, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.UserCreating.ToEventId(),
                    $"Creating {nameof(UserEntity)}: UserId {{UserId}}",
                    "FirstSeen")
                .WithoutException();

        public static void UserCurrentVersionOutOfDate(
                ILogger     logger,
                Snowflake   userId,
                long?       userVersionId)
            => _userCurrentVersionOutOfDate.Invoke(
                logger,
                userId,
                userVersionId);
        private static readonly Action<ILogger, Snowflake, long?> _userCurrentVersionOutOfDate
            = LoggerMessage.Define<Snowflake, long?>(
                    LogLevel.Debug,
                    EventType.UserCurrentVersionOutOfDate.ToEventId(),
                    $"Current {nameof(UserVersionEntity)} is out-of-date: UserId {{UserId}}, UserVersionId {{UserVersionId}}")
                .WithoutException();

        public static void UserCurrentVersionRetrieving(
                ILogger     logger,
                Snowflake   userId)
            => _userCurrentVersionRetrieving.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _userCurrentVersionRetrieving
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.UserCurrentVersionRetrieving.ToEventId(),
                    $"Retrieving current {nameof(UserVersionEntity)}: UserId {{UserId}}")
                .WithoutException();

        public static void UserCurrentVersionUpToDate(
                ILogger     logger,
                Snowflake   userId,
                long        userVersionId)
            => _userCurrentVersionUpToDate.Invoke(
                logger,
                userId,
                userVersionId);
        private static readonly Action<ILogger, Snowflake, long> _userCurrentVersionUpToDate
            = LoggerMessage.Define<Snowflake, long>(
                    LogLevel.Debug,
                    EventType.UserCurrentVersionUpToDate.ToEventId(),
                    $"Current {nameof(UserVersionEntity)} is up-to-date: UserId {{UserId}}, UserVersionId {{UserVersionId}}")
                .WithoutException();

        public static void UserMerged(
                ILogger logger,
                UserMergeModel model)
            => _userMerged.Invoke(
                logger,
                model.GuildId,
                model.UserId,
                model.AvatarHash,
                model.Discriminator,
                model.Nickname,
                model.Username);
        private static readonly Action<ILogger, Snowflake, Snowflake, Optional<string?>, Optional<ushort>, Optional<string?>, Optional<string>> _userMerged
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, Optional<string?>, Optional<ushort>, Optional<string?>, Optional<string>>(
                    LogLevel.Debug,
                    EventType.UserMerged.ToEventId(),
                    $"{nameof(UserMergeModel)} merged: GuildId {{GuildId}}, UserId {{UserId}}",
                    "AvatarHash",
                    "Discriminator",
                    "Nickname",
                    "Username")
                .WithoutException();

        public static void UserMerging(
                ILogger logger,
                UserMergeModel model)
            => _userMerging.Invoke(
                logger,
                model.GuildId,
                model.UserId,
                model.AvatarHash,
                model.Discriminator,
                model.Nickname,
                model.Username);
        private static readonly Action<ILogger, Snowflake, Snowflake, Optional<string?>, Optional<ushort>, Optional<string?>, Optional<string>> _userMerging
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, Optional<string?>, Optional<ushort>, Optional<string?>, Optional<string>>(
                    LogLevel.Debug,
                    EventType.UserMerging.ToEventId(),
                    $"Merging {nameof(UserMergeModel)}: GuildId {{GuildId}}, UserId {{UserId}}",
                    "AvatarHash",
                    "Discriminator",
                    "Nickname",
                    "Username")
                .WithoutException();

        public static void UserNotFound(
                ILogger     logger,
                Snowflake   userId)
            => _userNotFound.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _userNotFound
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.UserNotFound.ToEventId(),
                    $"{nameof(UserEntity)} not found: UserId {{UserId}}")
                .WithoutException();

        public static void UserRetrieved(
                ILogger     logger,
                Snowflake   userId)
            => _userRetrieved.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _userRetrieved
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.UserRetrieved.ToEventId(),
                    $"{nameof(UserEntity)} retrieved: UserId {{UserId}}")
                .WithoutException();

        public static void UserRetrieving(
                ILogger     logger,
                Snowflake   userId)
            => _userRetrieving.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _userRetrieving
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.UserRetrieving.ToEventId(),
                    $"Retrieving {nameof(UserEntity)}: UserId {{UserId}}")
                .WithoutException();

        public static void UserVersionCreated(
                ILogger             logger,
                UserVersionEntity   userVersion)
            => _userVersionCreated.Invoke(
                logger,
                userVersion.UserId,
                userVersion.Id,
                userVersion.PreviousVersionId,
                userVersion.AvatarHash,
                userVersion.Discriminator,
                userVersion.Username);
        private static readonly Action<ILogger, Snowflake, long, long?, string?, ushort, string> _userVersionCreated
            = StructuredLoggerMessage.Define<Snowflake, long, long?, string?, ushort, string>(
                    LogLevel.Debug,
                    EventType.UserVersionCreated.ToEventId(),
                    $"{nameof(UserVersionEntity)} created: UserId {{UserId}}, UserVersionId {{UserVersionId}}",
                    "PreviousVersionId",
                    "AvatarHash",
                    "Discriminator",
                    "Username")
                .WithoutException();

        public static void UserVersionCreating(
                ILogger             logger,
                UserVersionEntity   userVersion)
            => _userVersionCreating.Invoke(
                logger,
                userVersion.UserId,
                userVersion.PreviousVersionId,
                userVersion.AvatarHash,
                userVersion.Discriminator,
                userVersion.Username);
        private static readonly Action<ILogger, Snowflake, long?, string?, ushort, string> _userVersionCreating
            = StructuredLoggerMessage.Define<Snowflake, long?, string?, ushort, string>(
                    LogLevel.Debug,
                    EventType.UserVersionCreating.ToEventId(),
                    $"Creating {nameof(UserVersionEntity)}: UserId {{UserId}}, PreviousVersionId {{PreviousVersionId}}",
                    "AvatarHash",
                    "Discriminator",
                    "Username")
                .WithoutException();

        public static void UsersMerged(ILogger logger)
            => _usersMerged.Invoke(logger);
        private static readonly Action<ILogger> _usersMerged
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.UsersMerged.ToEventId(),
                    $"{nameof(UserMergeModel)} bulk-merged.")
                .WithoutException();

        public static void UsersMerging(ILogger logger)
            => _usersMerging.Invoke(logger);
        private static readonly Action<ILogger> _usersMerging
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.UsersMerging.ToEventId(),
                    $"Bulk-merging {nameof(UserMergeModel)}")
                .WithoutException();
    }
}
