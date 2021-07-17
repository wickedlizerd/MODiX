using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.Core;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Results;

using Modix.Data.Permissions;

namespace Modix.Business.Authorization
{
    internal static class AuthorizationLogMessages
    {
        private enum EventType
        {
            GrantedPermissionIdsRetrieving      = BusinessLogEventType.Authorization + 0x0100,
            GrantedPermissionIdsNotFound        = BusinessLogEventType.Authorization + 0x0200,
            GrantedPermissionIdsRetrieved       = BusinessLogEventType.Authorization + 0x0300,
            AdministratorIdentified             = BusinessLogEventType.Authorization + 0x0400,
            AllPermissionsRetrieving            = BusinessLogEventType.Authorization + 0x0500,
            AllPermissionsRetrieved             = BusinessLogEventType.Authorization + 0x0600,
            GuildMemberRetrieving               = BusinessLogEventType.Authorization + 0x0700,
            GuildMemberRetrievalFailed          = BusinessLogEventType.Authorization + 0x0800,
            GuildMemberRetrieved                = BusinessLogEventType.Authorization + 0x0900,
            GuildPermissionMappingsEnumerating  = BusinessLogEventType.Authorization + 0x0A00,
            GuildPermissionMappingsEnumerated   = BusinessLogEventType.Authorization + 0x0B00,
            GuildPermissionGranted              = BusinessLogEventType.Authorization + 0x0C00,
            GuildPermissionRevoked              = BusinessLogEventType.Authorization + 0x0D00,
            GuildPermissionMappingIgnored       = BusinessLogEventType.Authorization + 0x0E00,
            RolePermissionMappingsEnumerating   = BusinessLogEventType.Authorization + 0x0F00,
            RolePermissionMappingsEnumerated    = BusinessLogEventType.Authorization + 0x1000,
            RolePermissionGranted               = BusinessLogEventType.Authorization + 0x1100,
            RolePermissionRevoked               = BusinessLogEventType.Authorization + 0x1200,
            RolePermissionMappingIgnored        = BusinessLogEventType.Authorization + 0x1300,
            GrantedPermissionIdsCaching         = BusinessLogEventType.Authorization + 0x1400,
            GrantedPermissionIdsCached          = BusinessLogEventType.Authorization + 0x1500
        }

        public static void AdministratorIdentified(
                ILogger     logger,
                Snowflake   userId)
            => _administratorIdentified.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _administratorIdentified
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.AdministratorIdentified.ToEventId(),
                    "Administrator identified (UserId {UserId})")
                .WithoutException();

        public static void AllPermissionsRetrieved(
                ILogger logger,
                int     permissionCount)
            => _allPermissionsRetrieved.Invoke(
                logger,
                permissionCount);
        private static readonly Action<ILogger, int> _allPermissionsRetrieved
            = LoggerMessage.Define<int>(
                    LogLevel.Debug,
                    EventType.AllPermissionsRetrieved.ToEventId(),
                    "All permissions retrieved ({PermissionCount} permissions)")
                .WithoutException();

        public static void AllPermissionsRetrieving(ILogger logger)
            => _allPermissionsRetrieving.Invoke(logger);
        private static readonly Action<ILogger> _allPermissionsRetrieving
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.AllPermissionsRetrieving.ToEventId(),
                    "Retrieving all permissions")
                .WithoutException();

        public static void GrantedPermissionIdsCached(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId,
                int         permissionCount)
            => _grantedPermissionIdsCached.Invoke(
                logger,
                guildId,
                userId,
                permissionCount);
        private static readonly Action<ILogger, Snowflake, Snowflake, int> _grantedPermissionIdsCached
            = LoggerMessage.Define<Snowflake, Snowflake, int>(
                    LogLevel.Debug,
                    EventType.GrantedPermissionIdsCached.ToEventId(),
                    "Granted permissions cached (GuildId {GuildId}, UserId {UserId}, {PermissionCount} permissions)")
                .WithoutException();

        public static void GrantedPermissionIdsCaching(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId,
                int         permissionCount)
            => _grantedPermissionIdsCaching.Invoke(
                logger,
                guildId,
                userId,
                permissionCount);
        private static readonly Action<ILogger, Snowflake, Snowflake, int> _grantedPermissionIdsCaching
            = LoggerMessage.Define<Snowflake, Snowflake, int>(
                    LogLevel.Debug,
                    EventType.GrantedPermissionIdsCaching.ToEventId(),
                    "Caching granted permissions (GuildId {GuildId}, UserId {UserId}, {PermissionCount} permissions)")
                .WithoutException();

        public static void GrantedPermissionIdsNotFound(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId)
            => _grantedPermissionIdsNotFound.Invoke(
                logger,
                guildId,
                userId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _grantedPermissionIdsNotFound
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GrantedPermissionIdsNotFound.ToEventId(),
                    "Granted permissions not found (GuildId {GuildId}, UserId {UserId})")
                .WithoutException();

        public static void GrantedPermissionIdsRetrieved(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId,
                int         permissionCount)
            => _grantedPermissionIdsRetrieved.Invoke(
                logger,
                guildId,
                userId,
                permissionCount);
        private static readonly Action<ILogger, Snowflake, Snowflake, int> _grantedPermissionIdsRetrieved
            = LoggerMessage.Define<Snowflake, Snowflake, int>(
                    LogLevel.Debug,
                    EventType.GrantedPermissionIdsRetrieved.ToEventId(),
                    "Granted permissions retrieved ({PermissionCount} permissions, GuildId {GuildId}, UserId {UserId})")
                .WithoutException();

        public static void GrantedPermissionIdsRetrieving(
                ILogger     logger,
                Snowflake   guildId,
                Snowflake   userId)
            => _grantedPermissionIdsRetrieving.Invoke(
                logger,
                guildId,
                userId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _grantedPermissionIdsRetrieving
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GrantedPermissionIdsRetrieving.ToEventId(),
                    "Retrieving granted permissions (GuildId {GuildId}, UserId {UserId})")
                .WithoutException();

        public static void GuildMemberRetrievalFailed(
                ILogger         logger,
                IResultError    error)
            => _guildMemberRetrievalFailed.Invoke(
                logger,
                error.GetType().Name,
                error.Message);
        private static readonly Action<ILogger, string, string> _guildMemberRetrievalFailed
            = LoggerMessage.Define<string, string>(
                    LogLevel.Error,
                    EventType.GuildMemberRetrievalFailed.ToEventId(),
                    "Guild member retrieval failed: {ErrorType}: {ErrorMessage}")
                .WithoutException();

        public static void GuildMemberRetrieved(
                ILogger                 logger,
                Snowflake               guildId,
                Snowflake               userId,
                IDiscordPermissionSet   permissions,
                int                     roleCount)
            => _guildMemberRetrieved.Invoke(
                logger,
                guildId,
                userId,
                permissions,
                roleCount);
        private static readonly Action<ILogger, Snowflake, Snowflake, IDiscordPermissionSet, int> _guildMemberRetrieved
            = StructuredLoggerMessage.Define<Snowflake, Snowflake, IDiscordPermissionSet, int>(
                    LogLevel.Debug,
                    EventType.GuildMemberRetrieved.ToEventId(),
                    "Guild member retrieved (GuildId {GuildId}, UserId {UserId})",
                    "Permissions",
                    "RoleCount")
                .WithoutException();

        public static void GuildMemberRetrieving(
                ILogger                 logger,
                Snowflake               guildId,
                Snowflake               userId)
            => _guildMemberRetrieving.Invoke(
                logger,
                guildId,
                userId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _guildMemberRetrieving
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildMemberRetrieving.ToEventId(),
                    "Retrieving guild member (GuildId {GuildId}, UserId {UserId})")
                .WithoutException();

        public static void GuildPermissionGranted(
                ILogger                                 logger,
                GuildPermissionMappingDefinitionModel   mapping)
            => _guildPermissionGranted.Invoke(
                logger,
                mapping.PermissionId,
                mapping.GuildPermission,
                mapping.GuildId);
        private static readonly Action<ILogger, int, DiscordPermission, Snowflake> _guildPermissionGranted
            = LoggerMessage.Define<int, DiscordPermission, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildPermissionGranted.ToEventId(),
                    "Permission {PermissionId} granted, by virtue of permission {GuildPermission} in guild {GuildId}")
                .WithoutException();

        public static void GuildPermissionMappingIgnored(
                ILogger                                 logger,
                GuildPermissionMappingDefinitionModel mapping)
            => _guildPermissionMappingIgnored.Invoke(
                logger,
                mapping.PermissionId,
                mapping.GuildPermission,
                mapping.GuildId);
        private static readonly Action<ILogger, int, DiscordPermission, Snowflake> _guildPermissionMappingIgnored
            = LoggerMessage.Define<int, DiscordPermission, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildPermissionGranted.ToEventId(),
                    "Permission {PermissionId} ignored, by virtue of permission {GuildPermission} in guild {GuildId}")
                .WithoutException();

        public static void GuildPermissionMappingsEnumerated(
                ILogger     logger,
                Snowflake   guildId)
            => _guildPermissionMappingsEnumerated.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _guildPermissionMappingsEnumerated
            = StructuredLoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildPermissionMappingsEnumerated.ToEventId(),
                    "Guild permission mappings enumerated",
                    "GuildId")
                .WithoutException();

        public static void GuildPermissionMappingsEnumerating(
                ILogger     logger,
                Snowflake   guildId)
            => _guildPermissionMappingsEnumerating.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _guildPermissionMappingsEnumerating
            = StructuredLoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildPermissionMappingsEnumerating.ToEventId(),
                    "Enumerating guild permission mappings",
                    "GuildId")
                .WithoutException();

        public static void GuildPermissionRevoked(
                ILogger                                 logger,
                GuildPermissionMappingDefinitionModel   mapping)
            => _guildPermissionRevoked.Invoke(
                logger,
                mapping.PermissionId,
                mapping.GuildPermission,
                mapping.GuildId);
        private static readonly Action<ILogger, int, DiscordPermission, Snowflake> _guildPermissionRevoked
            = LoggerMessage.Define<int, DiscordPermission, Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildPermissionRevoked.ToEventId(),
                    "Permission {PermissionId} revoked, by virtue of permission {GuildPermission} in guild {GuildId}")
                .WithoutException();

        public static void RolePermissionGranted(
                ILogger                                 logger,
                RolePermissionMappingDefinitionModel    mapping)
            => _rolePermissionGranted.Invoke(
                logger,
                mapping.PermissionId,
                mapping.RoleId,
                mapping.GuildId);
        private static readonly Action<ILogger, int, Snowflake, Snowflake> _rolePermissionGranted
            = LoggerMessage.Define<int, Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.RolePermissionGranted.ToEventId(),
                    "Permission {PermissionId} granted, by virtue of role {RoleId} in guild {GuildId}")
                .WithoutException();

        public static void RolePermissionMappingIgnored(
                ILogger                                 logger,
                RolePermissionMappingDefinitionModel    mapping)
            => _rolePermissionMappingIgnored.Invoke(
                logger,
                mapping.PermissionId,
                mapping.RoleId,
                mapping.GuildId);
        private static readonly Action<ILogger, int, Snowflake, Snowflake> _rolePermissionMappingIgnored
            = LoggerMessage.Define<int, Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.RolePermissionGranted.ToEventId(),
                    "Permission {PermissionId} ignored, by virtue of role {RoleId} in guild {GuildId}")
                .WithoutException();

        public static void RolePermissionMappingsEnumerated(
                ILogger     logger,
                Snowflake   guildId)
            => _rolePermissionMappingsEnumerated.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _rolePermissionMappingsEnumerated
            = StructuredLoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.RolePermissionMappingsEnumerated.ToEventId(),
                    "Role permission mappings enumerated",
                    "GuildId")
                .WithoutException();

        public static void RolePermissionMappingsEnumerating(
                ILogger     logger,
                Snowflake   guildId)
            => _rolePermissionMappingsEnumerating.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _rolePermissionMappingsEnumerating
            = StructuredLoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.RolePermissionMappingsEnumerating.ToEventId(),
                    "Enumerating role permission mappings",
                    "GuildId")
                .WithoutException();

        public static void RolePermissionRevoked(
                ILogger                                 logger,
                RolePermissionMappingDefinitionModel    mapping)
            => _rolePermissionRevoked.Invoke(
                logger,
                mapping.PermissionId,
                mapping.RoleId,
                mapping.GuildId);
        private static readonly Action<ILogger, int, Snowflake, Snowflake> _rolePermissionRevoked
            = LoggerMessage.Define<int, Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.RolePermissionRevoked.ToEventId(),
                    "Permission {PermissionId} revoked, by virtue of role {RoleId} in guild {GuildId}")
                .WithoutException();
    }
}
