using System;

using Microsoft.Extensions.Logging;

using Remora.Discord.Core;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Results;

using Modix.Data.Permissions;

namespace Modix.Business.Authorization
{
    internal static partial class AuthorizationLogMessages
    {
        [LoggerMessage(
            EventId = 0x7AF87C65,
            Level   = LogLevel.Debug,
            Message = "Administrator identified (UserId {UserId})")]
        public static partial void AdministratorIdentified(
            ILogger     logger,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x1360FE21,
            Level   = LogLevel.Debug,
            Message = "All permissions retrieved ({PermissionCount} permissions)")]
        public static partial void AllPermissionsRetrieved(
            ILogger logger,
            int     permissionCount);

        [LoggerMessage(
            EventId = 0x02B4C35B,
            Level   = LogLevel.Debug,
            Message = "Retrieving all permissions")]
        public static partial void AllPermissionsRetrieving(ILogger logger);

        [LoggerMessage(
            EventId = 0x32B57E30,
            Level   = LogLevel.Debug,
            Message = "Granted permissions cached (GuildId {GuildId}, UserId {UserId}, {PermissionCount} permissions)")]
        public static partial void GrantedPermissionIdsCached(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId,
            int         permissionCount);

        [LoggerMessage(
            EventId = 0x736EC4A3,
            Level   = LogLevel.Debug,
            Message = "Caching granted permissions (GuildId {GuildId}, UserId {UserId}, {PermissionCount} permissions)")]
        public static partial void GrantedPermissionIdsCaching(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId,
            int         permissionCount);

        [LoggerMessage(
            EventId = 0x6E7F3DC4,
            Level   = LogLevel.Debug,
            Message = "Granted permissions not found (GuildId {GuildId}, UserId {UserId})")]
        public static partial void GrantedPermissionIdsNotFound(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x08273A3D,
            Level   = LogLevel.Debug,
            Message = "Granted permissions retrieved ({PermissionCount} permissions, GuildId {GuildId}, UserId {UserId})")]
        public static partial void GrantedPermissionIdsRetrieved(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId,
            int         permissionCount);

        [LoggerMessage(
            EventId = 0x6D1296C2,
            Level   = LogLevel.Debug,
            Message = "Retrieving granted permissions (GuildId {GuildId}, UserId {UserId})")]
        public static partial void GrantedPermissionIdsRetrieving(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId);

        public static void GuildMemberRetrievalFailed(
                ILogger         logger,
                IResultError    error)
            => GuildMemberRetrievalFailed(
                logger,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);

        [LoggerMessage(
            EventId = 0x68817ADE,
            Level   = LogLevel.Error,
            Message = "Guild member retrieval failed: {ErrorType}: {ErrorMessage}")]
        private static partial void GuildMemberRetrievalFailed(
            ILogger     logger,
            string      errorType,
            string      errorMessage,
            Exception?  exception);

        [LoggerMessage(
            EventId = 0x76D95776,
            Level   = LogLevel.Debug,
            Message = "Guild member retrieved (GuildId {GuildId}, UserId {UserId})")]
        public static partial void GuildMemberRetrieved(
            ILogger                 logger,
            Snowflake               guildId,
            Snowflake               userId,
            IDiscordPermissionSet   permissions,
            int                     roleCount);

        [LoggerMessage(
            EventId = 0x1B67C894,
            Level   = LogLevel.Debug,
            Message = "Retrieving guild member (GuildId {GuildId}, UserId {UserId})")]
        public static partial void GuildMemberRetrieving(
            ILogger     logger,
            Snowflake   guildId,
            Snowflake   userId);

        public static void GuildPermissionGranted(
                ILogger                                 logger,
                GuildPermissionMappingDefinitionModel   mapping)
            => GuildPermissionGranted(
                logger,
                mapping.PermissionId,
                mapping.GuildPermission,
                mapping.GuildId);

        [LoggerMessage(
            EventId = 0x7EB09F3B,
            Level   = LogLevel.Debug,
            Message = "Permission {PermissionId} granted, by virtue of permission {GuildPermission} in guild {GuildId}")]
        private static partial void GuildPermissionGranted(
            ILogger             logger,
            int                 permissionId,
            DiscordPermission   guildPermission,
            Snowflake           guildId);

        public static void GuildPermissionMappingIgnored(
                ILogger                                 logger,
                GuildPermissionMappingDefinitionModel   mapping)
            => GuildPermissionMappingIgnored(
                logger,
                mapping.PermissionId,
                mapping.GuildPermission,
                mapping.GuildId);

        [LoggerMessage(
            EventId = 0x5B296087,
            Level   = LogLevel.Debug,
            Message = "Permission {PermissionId} ignored, by virtue of permission {GuildPermission} in guild {GuildId}")]
        private static partial void GuildPermissionMappingIgnored(
            ILogger             logger,
            int                 permissionId,
            DiscordPermission   guildPermission,
            Snowflake           guildId);

        [LoggerMessage(
            EventId = 0x1FD85DA1,
            Level   = LogLevel.Debug,
            Message = "Guild permission mappings enumerated")]
        public static partial void GuildPermissionMappingsEnumerated(
            ILogger     logger,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x70EF71CC,
            Level   = LogLevel.Debug,
            Message = "Enumerating guild permission mappings")]
        public static partial void GuildPermissionMappingsEnumerating(
            ILogger     logger,
            Snowflake   guildId);

        public static void GuildPermissionRevoked(
                ILogger                                 logger,
                GuildPermissionMappingDefinitionModel   mapping)
            => GuildPermissionRevoked(
                logger,
                mapping.PermissionId,
                mapping.GuildPermission,
                mapping.GuildId);
        [LoggerMessage(
            EventId = 0x3D521B85,
            Level   = LogLevel.Debug,
            Message = "Permission {PermissionId} revoked, by virtue of permission {GuildPermission} in guild {GuildId}")]
        private static partial void GuildPermissionRevoked(
                ILogger             logger,
                int                 permissionId,
                DiscordPermission   guildPermission,
                Snowflake           guildId);

        public static void RolePermissionGranted(
                ILogger                                 logger,
                RolePermissionMappingDefinitionModel    mapping)
            => RolePermissionGranted(
                logger,
                mapping.PermissionId,
                mapping.RoleId,
                mapping.GuildId);

        [LoggerMessage(
            EventId = 0x603731FA,
            Level   = LogLevel.Debug,
            Message = "Permission {PermissionId} granted, by virtue of role {RoleId} in guild {GuildId}")]
        private static partial void RolePermissionGranted(
            ILogger     logger,
            int         permissionId,
            Snowflake   roleId,
            Snowflake   guildId);

        public static void RolePermissionMappingIgnored(
                ILogger                                 logger,
                RolePermissionMappingDefinitionModel    mapping)
            => RolePermissionMappingIgnored(
                logger,
                mapping.PermissionId,
                mapping.RoleId,
                mapping.GuildId);

        [LoggerMessage(
            EventId = 0x06049CD7,
            Level   = LogLevel.Debug,
            Message = "Permission {PermissionId} ignored, by virtue of role {RoleId} in guild {GuildId}")]
        private static partial void RolePermissionMappingIgnored(
            ILogger     logger,
            int         permissionId,
            Snowflake   roleId,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x0E671952,
            Level   = LogLevel.Debug,
            Message = "Role permission mappings enumerated")]
        public static partial void RolePermissionMappingsEnumerated(
            ILogger     logger,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x70E306ED,
            Level   = LogLevel.Debug,
            Message = "Enumerating role permission mappings")]
        public static partial void RolePermissionMappingsEnumerating(
            ILogger     logger,
            Snowflake   guildId);

        public static void RolePermissionRevoked(
                ILogger                                 logger,
                RolePermissionMappingDefinitionModel    mapping)
            => RolePermissionRevoked(
                logger,
                mapping.PermissionId,
                mapping.RoleId,
                mapping.GuildId);

        [LoggerMessage(
            EventId = 0x5F4E50CA,
            Level   = LogLevel.Debug,
            Message = "Permission {PermissionId} revoked, by virtue of role {RoleId} in guild {GuildId}")]
        public static partial void RolePermissionRevoked(
            ILogger     logger,
            int         permissionId,
            Snowflake   roleId,
            Snowflake   guildId);
    }
}
