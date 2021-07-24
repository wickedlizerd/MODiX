using Microsoft.Extensions.Logging;

using Remora.Discord.Core;

namespace Modix.Data.Permissions
{
    internal static partial class PermissionsLogMessages
    {
        [LoggerMessage(
            EventId = 0x509A73A7,
            Level   = LogLevel.Debug,
            Message = "Adding GuildId ({GuildId}) clause to guild permission mappings enumeration")]
        public static partial void GuildPermissionMappingsEnumerationAddingGuildIdClause(
            ILogger     logger,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x69736EE7,
            Level   = LogLevel.Debug,
            Message = "Adding IsDeleted ({IsDeleted}) clause to guild permission mappings enumeration")]
        public static partial void GuildPermissionMappingsEnumerationAddingIsDeletedClause(
            ILogger logger,
            bool    isDeleted);

        public static void GuildPermissionMappingsEnumerationBuilding(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => GuildPermissionMappingsEnumerationBuilding(
                logger,
                guildId.ToString(),
                isDeleted.ToString());

        [LoggerMessage(
            EventId = 0x1FFAC20D,
            Level   = LogLevel.Debug,
            Message = "Building guild permission mappings enumeration")]
        private static partial void GuildPermissionMappingsEnumerationBuilding(
            ILogger logger,
            string  guildId,
            string  isDeleted);

        public static void GuildPermissionMappingsEnumerationBuilt(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => GuildPermissionMappingsEnumerationBuilt(
                logger,
                guildId.ToString(),
                isDeleted.ToString());

        [LoggerMessage(
            EventId = 0x117C12E7,
            Level   = LogLevel.Debug,
            Message = "Building guild permission mappings enumeration")]
        private static partial void GuildPermissionMappingsEnumerationBuilt(
            ILogger logger,
            string  guildId,
            string  isDeleted);

        [LoggerMessage(
            EventId = 0x4BFB5C94,
            Level   = LogLevel.Debug,
            Message = "Finalizing guild permission mappings enumeration")]
        public static partial void GuildPermissionMappingsEnumerationFinalizing(ILogger logger);

        [LoggerMessage(
            EventId = 0x2671BCA7,
            Level   = LogLevel.Debug,
            Message = "Building permission IDs enumeration")]
        public static partial void PermissionIdsEnumerationBuilding(ILogger logger);

        [LoggerMessage(
            EventId = 0x2E81AD6F,
            Level   = LogLevel.Debug,
            Message = "Building permission IDs enumeration")]
        public static partial void PermissionIdsEnumerationBuilt(ILogger logger);

        [LoggerMessage(
            EventId = 0x45B9C920,
            Level   = LogLevel.Debug,
            Message = "Adding GuildId ({GuildId}) clause to role permission mappings enumeration")]
        public static partial void RolePermissionMappingsEnumerationAddingGuildIdClause(
            ILogger     logger,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x1060A2B6,
            Level   = LogLevel.Debug,
            Message = "Adding IsDeleted ({IsDeleted}) clause to role permission mappings enumeration")]
        public static partial void RolePermissionMappingsEnumerationAddingIsDeletedClause(
            ILogger logger,
            bool    isDeleted);

        public static void RolePermissionMappingsEnumerationBuilding(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => RolePermissionMappingsEnumerationBuilding(
                logger,
                guildId.ToString(),
                isDeleted.ToString());

        [LoggerMessage(
            EventId = 0x6BEA9C48,
            Level   = LogLevel.Debug,
            Message = "Building role permission mappings enumeration")]
        private static partial void RolePermissionMappingsEnumerationBuilding(
            ILogger logger,
            string  guildId,
            string  isDeleted);

        public static void RolePermissionMappingsEnumerationBuilt(
                ILogger             logger,
                Optional<Snowflake> guildId,
                Optional<bool>      isDeleted)
            => RolePermissionMappingsEnumerationBuilt(
                logger,
                guildId.ToString(),
                isDeleted.ToString());

        [LoggerMessage(
            EventId = 0x4654F6B1,
            Level   = LogLevel.Debug,
            Message = "Building role permission mappings enumeration")]
        private static partial void RolePermissionMappingsEnumerationBuilt(
            ILogger logger,
            string  guildId,
            string  isDeleted);

        [LoggerMessage(
            EventId = 0x4D207C82,
            Level   = LogLevel.Debug,
            Message = "Finalizing role permission mappings enumeration")]
        public static partial void RolePermissionMappingsEnumerationFinalizing(ILogger logger);
    }
}
