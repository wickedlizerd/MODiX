using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.Core;

namespace Modix.Data.Permissions
{
    internal static class PermissionsLogMessages
    {
        private enum EventType
        {
            PermissionIdsEnumerationBuilding                        = DataLogEventType.Permissions + 0x0100,
            PermissionIdsEnumerationBuilt                           = DataLogEventType.Permissions + 0x0200,
            GuildPermissionMappingsEnumerationBuilding              = DataLogEventType.Permissions + 0x0300,
            GuildPermissionMappingsEnumerationAddingGuildIdClause   = DataLogEventType.Permissions + 0x0400,
            GuildPermissionMappingsEnumerationAddingIsDeletedClause = DataLogEventType.Permissions + 0x0500,
            GuildPermissionMappingsEnumerationFinalizing            = DataLogEventType.Permissions + 0x0600,
            GuildPermissionMappingsEnumerationBuilt                 = DataLogEventType.Permissions + 0x0700,
            RolePermissionMappingsEnumerationBuilding               = DataLogEventType.Permissions + 0x0800,
            RolePermissionMappingsEnumerationAddingGuildIdClause    = DataLogEventType.Permissions + 0x0900,
            RolePermissionMappingsEnumerationAddingIsDeletedClause  = DataLogEventType.Permissions + 0x0A00,
            RolePermissionMappingsEnumerationFinalizing             = DataLogEventType.Permissions + 0x0B00,
            RolePermissionMappingsEnumerationBuilt                  = DataLogEventType.Permissions + 0x0C00
        }

        public static void GuildPermissionMappingsEnumerationAddingGuildIdClause(
                ILogger     logger,
                Snowflake   guildId)
            => _guildPermissionMappingsEnumerationAddingGuildIdClause.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _guildPermissionMappingsEnumerationAddingGuildIdClause
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildPermissionMappingsEnumerationAddingGuildIdClause.ToEventId(),
                    "Adding GuildId ({GuildId}) clause to guild permission mappings enumeration")
                .WithoutException();

        public static void GuildPermissionMappingsEnumerationAddingIsDeletedClause(
                ILogger logger,
                bool    isDeleted)
            => _guildPermissionMappingsEnumerationAddingIsDeletedClause.Invoke(
                logger,
                isDeleted);
        private static readonly Action<ILogger, bool> _guildPermissionMappingsEnumerationAddingIsDeletedClause
            = LoggerMessage.Define<bool>(
                    LogLevel.Debug,
                    EventType.GuildPermissionMappingsEnumerationAddingIsDeletedClause.ToEventId(),
                    "Adding IsDeleted ({IsDeleted}) clause to guild permission mappings enumeration")
                .WithoutException();

        public static void GuildPermissionMappingsEnumerationBuilding(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => _guildPermissionMappingsEnumerationBuilding.Invoke(
                logger,
                guildId.ToString(),
                isDeleted.ToString());
        private static readonly Action<ILogger, string, string> _guildPermissionMappingsEnumerationBuilding
            = StructuredLoggerMessage.Define<string, string>(
                    LogLevel.Debug,
                    EventType.GuildPermissionMappingsEnumerationBuilding.ToEventId(),
                    "Building guild permission mappings enumeration",
                    "GuildId",
                    "IsDeleted")
                .WithoutException();

        public static void GuildPermissionMappingsEnumerationBuilt(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => _guildPermissionMappingsEnumerationBuilt.Invoke(
                logger,
                guildId.ToString(),
                isDeleted.ToString());
        private static readonly Action<ILogger, string, string> _guildPermissionMappingsEnumerationBuilt
            = StructuredLoggerMessage.Define<string, string>(
                    LogLevel.Debug,
                    EventType.GuildPermissionMappingsEnumerationBuilt.ToEventId(),
                    "Building guild permission mappings enumeration",
                    "GuildId",
                    "IsDeleted")
                .WithoutException();

        public static void GuildPermissionMappingsEnumerationFinalizing(ILogger logger)
            => _guildPermissionMappingsEnumerationFinalizing.Invoke(logger);
        private static readonly Action<ILogger> _guildPermissionMappingsEnumerationFinalizing
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.GuildPermissionMappingsEnumerationFinalizing.ToEventId(),
                    "Finalizing guild permission mappings enumeration")
                .WithoutException();

        public static void PermissionIdsEnumerationBuilding(ILogger logger)
            => _permissionIdsEnumerationBuilding.Invoke(logger);
        private static readonly Action<ILogger> _permissionIdsEnumerationBuilding
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.PermissionIdsEnumerationBuilding.ToEventId(),
                    "Building permission IDs enumeration")
                .WithoutException();

        public static void PermissionIdsEnumerationBuilt(ILogger logger)
            => _permissionIdsEnumerationBuilt.Invoke(logger);
        private static readonly Action<ILogger> _permissionIdsEnumerationBuilt
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.PermissionIdsEnumerationBuilt.ToEventId(),
                    "Building permission IDs enumeration")
                .WithoutException();

        public static void RolePermissionMappingsEnumerationAddingGuildIdClause(
                ILogger     logger,
                Snowflake   guildId)
            => _rolePermissionMappingsEnumerationAddingGuildIdClause.Invoke(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _rolePermissionMappingsEnumerationAddingGuildIdClause
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.RolePermissionMappingsEnumerationAddingGuildIdClause.ToEventId(),
                    "Adding GuildId ({GuildId}) clause to role permission mappings enumeration")
                .WithoutException();

        public static void RolePermissionMappingsEnumerationAddingIsDeletedClause(
                ILogger logger,
                bool    isDeleted)
            => _rolePermissionMappingsEnumerationAddingIsDeletedClause.Invoke(
                logger,
                isDeleted);
        private static readonly Action<ILogger, bool> _rolePermissionMappingsEnumerationAddingIsDeletedClause
            = LoggerMessage.Define<bool>(
                    LogLevel.Debug,
                    EventType.RolePermissionMappingsEnumerationAddingIsDeletedClause.ToEventId(),
                    "Adding IsDeleted ({IsDeleted}) clause to role permission mappings enumeration")
                .WithoutException();

        public static void RolePermissionMappingsEnumerationBuilding(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => _rolePermissionMappingsEnumerationBuilding.Invoke(
                logger,
                guildId.ToString(),
                isDeleted.ToString());
        private static readonly Action<ILogger, string, string> _rolePermissionMappingsEnumerationBuilding
            = StructuredLoggerMessage.Define<string, string>(
                    LogLevel.Debug,
                    EventType.RolePermissionMappingsEnumerationBuilding.ToEventId(),
                    "Building role permission mappings enumeration",
                    "GuildId",
                    "IsDeleted")
                .WithoutException();

        public static void RolePermissionMappingsEnumerationBuilt(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => _rolePermissionMappingsEnumerationBuilt.Invoke(
                logger,
                guildId.ToString(),
                isDeleted.ToString());
        private static readonly Action<ILogger, string, string> _rolePermissionMappingsEnumerationBuilt
            = StructuredLoggerMessage.Define<string, string>(
                    LogLevel.Debug,
                    EventType.RolePermissionMappingsEnumerationBuilt.ToEventId(),
                    "Building role permission mappings enumeration",
                    "GuildId",
                    "IsDeleted")
                .WithoutException();

        public static void RolePermissionMappingsEnumerationFinalizing(ILogger logger)
            => _rolePermissionMappingsEnumerationFinalizing.Invoke(logger);
        private static readonly Action<ILogger> _rolePermissionMappingsEnumerationFinalizing
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.RolePermissionMappingsEnumerationFinalizing.ToEventId(),
                    "Finalizing role permission mappings enumeration")
                .WithoutException();
    }
}
