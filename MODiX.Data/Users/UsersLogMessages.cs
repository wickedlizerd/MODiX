using System;

using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

namespace Modix.Data.Users
{
    internal static partial class UsersLogMessages
    {
        public static void GuildUserCreated(
                ILogger         logger,
                GuildUserEntity guildUser)
            => GuildUserCreated(
                logger,
                guildUser.GuildId,
                guildUser.UserId,
                guildUser.FirstSeen,
                guildUser.LastSeen);

        [LoggerMessage(
            EventId = 0x126451F3,
            Level   = LogLevel.Debug,
            Message = "Guild user created: GuildId {GuildId}, UserId {UserId}")]
        private static partial void GuildUserCreated(
            ILogger         logger,
            Snowflake       guildId,
            Snowflake       userId,
            DateTimeOffset  firstSeen,
            DateTimeOffset  lastSeen);

        public static void GuildUserCreating(
                ILogger         logger,
                GuildUserEntity guildUser)
            => GuildUserCreating(
                logger,
                guildUser.GuildId,
                guildUser.UserId,
                guildUser.FirstSeen,
                guildUser.LastSeen);

        [LoggerMessage(
            EventId = 0x1C4B4FD5,
            Level   = LogLevel.Debug,
            Message = "Creating guild user: GuildId {GuildId}, UserId {UserId}")]
        private static partial void GuildUserCreating(
            ILogger         logger,
            Snowflake       guildId,
            Snowflake       userId,
            DateTimeOffset  firstSeen,
            DateTimeOffset  lastSeen);

        [LoggerMessage(
            EventId = 0x7B57BFCA,
            Level   = LogLevel.Debug,
            Message = "Current guild user version is out-of-date: GuildId {GuildId}, UserId {UserId}, GuildUserVersionId {GuildUserVersionId}")]
        public static partial void GuildUserCurrentVersionOutOfDate(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId,
            long?       guildUserVersionId);

        [LoggerMessage(
            EventId = 0x29456D1F,
            Level   = LogLevel.Debug,
            Message = "Retrieving current guild user version: GuildId {GuildId}, UserId {UserID}")]
        public static partial void GuildUserCurrentVersionRetrieving(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x0EDEB285,
            Level   = LogLevel.Debug,
            Message = "Current guild user version is up-to-date: GuildId {GuildId}, UserId {UserId}, GuildUserVersionId {GuildUserVersionId}")]
        public static partial void GuildUserCurrentVersionUpToDate(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId,
            long        guildUserVersionId);

        [LoggerMessage(
            EventId = 0x5E503951,
            Level   = LogLevel.Debug,
            Message = "Guild user not found: GuildId {GuildId}, UserId {UserId}")]
        public static partial void GuildUserNotFound(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x5B1579E6,
            Level   = LogLevel.Debug,
            Message = "Guild user retrieved: GuildId {GuildId}, UserId {UserId}")]
        public static partial void GuildUserRetrieved(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x064AD16D,
            Level   = LogLevel.Debug,
            Message = "Retrieving guild user: GuildId {GuildId}, UserId {UserId}")]
        public static partial void GuildUserRetrieving(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId);

        public static void GuildUserVersionCreated(
                ILogger                 logger,
                GuildUserVersionEntity  guildUserVersion)
            => GuildUserVersionCreated(
                logger,
                guildUserVersion.GuildId,
                guildUserVersion.UserId,
                guildUserVersion.Id,
                guildUserVersion.PreviousVersionId,
                guildUserVersion.Created,
                guildUserVersion.Nickname);

        [LoggerMessage(
            EventId = 0x0B313259,
            Level   = LogLevel.Debug,
            Message = "Guild user version created: GuildId {GuildId}, UserId {UserId}, GuildUserVersionId {GuildUserVersionId}")]
        private static partial void GuildUserVersionCreated(
            ILogger         logger,
            Snowflake       guildId,
            Snowflake       userId,
            long            guildUserVersionId,
            long?           previousVersionId,
            DateTimeOffset  created,
            string?         nickname);

        public static void GuildUserVersionCreating(
                ILogger                 logger,
                GuildUserVersionEntity  guildUserVersion)
            => GuildUserVersionCreating(
                logger,
                guildUserVersion.GuildId,
                guildUserVersion.UserId,
                guildUserVersion.PreviousVersionId,
                guildUserVersion.Created,
                guildUserVersion.Nickname);

        [LoggerMessage(
            EventId = 0x391C9353,
            Level   = LogLevel.Debug,
            Message = "Creating guild user version: GuildId {GuildId}, UserId {UserId}, PreviousVersionId {PreviousVersionId}")]
        private static partial void GuildUserVersionCreating(
            ILogger         logger,
            Snowflake       guildId,
            Snowflake       userId,
            long?           previousVersionId,
            DateTimeOffset  created,
            string?         nickname);

        public static void UserCreated(
                ILogger     logger,
                UserEntity  user)
            => UserCreated(
                logger,
                user.Id,
                user.FirstSeen,
                user.LastSeen);

        [LoggerMessage(
            EventId = 0x1B12A410,
            Level   = LogLevel.Debug,
            Message = "User created: UserId {UserId}")]
        private static partial void UserCreated(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  firstSeen,
            DateTimeOffset  lastSeen);

        public static void UserCreating(
                ILogger     logger,
                UserEntity  user)
            => UserCreating(
                logger,
                user.Id,
                user.FirstSeen,
                user.LastSeen);

        [LoggerMessage(
            EventId = 0x16DE8503,
            Level   = LogLevel.Debug,
            Message = "Creating user: UserId {UserId}")]
        private static partial void UserCreating(
            ILogger         logger,
            Snowflake       userId,
            DateTimeOffset  firstSeen,
            DateTimeOffset  lastSeen);

        [LoggerMessage(
            EventId = 0x7D11560B,
            Level   = LogLevel.Debug,
            Message = "Current user version is out-of-date: UserId {UserId}, UserVersionId {UserVersionId}")]
        public static partial void UserCurrentVersionOutOfDate(
            ILogger     logger,
            Snowflake   userId,
            long?       userVersionId);

        [LoggerMessage(
            EventId = 0x4056691F,
            Level   = LogLevel.Debug,
            Message = "Retrieving current user version: UserId {UserId}")]
        public static partial void UserCurrentVersionRetrieving(
            ILogger     logger,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x395017FB,
            Level   = LogLevel.Debug,
            Message = "Current user version is up-to-date: UserId {UserId}, UserVersionId {UserVersionId}")]
        public static partial void UserCurrentVersionUpToDate(
            ILogger     logger,
            Snowflake   userId,
            long        userVersionId);

        public static void UserMerged(
                ILogger         logger,
                UserMergeModel  model)
            => UserMerged(
                logger,
                model.GuildId,
                model.UserId,
                model.AvatarHash.ToString(),
                model.Discriminator.ToString(),
                model.Nickname.ToString(),
                model.Username.ToString());

        [LoggerMessage(
            EventId = 0x3AB5B788,
            Level   = LogLevel.Debug,
            Message = "User data merged: GuildId {GuildId}, UserId {UserId}")]
        private static partial void UserMerged(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId,
            string      avatarHash,
            string      discriminator,
            string      nickname,
            string      username);

        public static void UserMerging(
                ILogger         logger,
                UserMergeModel  model)
            => UserMerging(
                logger,
                model.GuildId,
                model.UserId,
                model.AvatarHash.ToString(),
                model.Discriminator.ToString(),
                model.Nickname.ToString(),
                model.Username.ToString());

        [LoggerMessage(
            EventId = 0x3FADEE39,
            Level   = LogLevel.Debug,
            Message = "Merging user data: GuildId {GuildId}, UserId {UserId}")]
        private static partial void UserMerging(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId,
            string      avatarHash,
            string      discriminator,
            string      nickname,
            string      username);

        [LoggerMessage(
            EventId = 0x0374094F,
            Level   = LogLevel.Debug,
            Message = "User not found: UserId {UserId}")]
        public static partial void UserNotFound(
            ILogger     logger,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x4756279B,
            Level   = LogLevel.Debug,
            Message = "User retrieved: UserId {UserId}")]
        public static partial void UserRetrieved(
            ILogger     logger,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x068B3659,
            Level   = LogLevel.Debug,
            Message = "Retrieving user: UserId {UserId}")]
        public static partial void UserRetrieving(
            ILogger     logger,
            Snowflake   userId);

        public static void UserVersionCreated(
                ILogger             logger,
                UserVersionEntity   userVersion)
            => UserVersionCreated(
                logger,
                userVersion.UserId,
                userVersion.Id,
                userVersion.PreviousVersionId,
                userVersion.AvatarHash,
                userVersion.Discriminator,
                userVersion.Username);

        [LoggerMessage(
            EventId = 0x7A576AFB,
            Level   = LogLevel.Debug,
            Message = "User version created: UserId {UserId}, UserVersionId {UserVersionId}")]
        private static partial void UserVersionCreated(
            ILogger     logger,
            Snowflake   userId,
            long        userVersionId,
            long?       previousVersionId,
            string?     avatarHash,
            ushort      discriminator,
            string      username);

        public static void UserVersionCreating(
                ILogger             logger,
                UserVersionEntity   userVersion)
            => UserVersionCreating(
                logger,
                userVersion.UserId,
                userVersion.PreviousVersionId,
                userVersion.AvatarHash,
                userVersion.Discriminator,
                userVersion.Username);

        [LoggerMessage(
            EventId = 0x75C20591,
            Level   = LogLevel.Debug,
            Message = "Creating user version: UserId {UserId}, PreviousVersionId {PreviousVersionId}")]
        private static partial void UserVersionCreating(
            ILogger     logger,
            Snowflake   userId,
            long?       previousVersionId,
            string?     avatarHash,
            ushort      discriminator,
            string      username);

        [LoggerMessage(
            EventId = 0x772B39A5,
            Level   = LogLevel.Debug,
            Message = "User data bulk-merged.")]
        public static partial void UsersMerged(ILogger logger);

        [LoggerMessage(
            EventId = 0x13891BB1,
            Level   = LogLevel.Debug,
            Message = "Bulk-merging user data")]
        public static partial void UsersMerging(ILogger logger);
    }
}
