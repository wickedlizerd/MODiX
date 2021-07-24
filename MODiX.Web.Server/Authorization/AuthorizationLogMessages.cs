using System;

using Microsoft.Extensions.Logging;

using Remora.Discord.Core;
using Remora.Results;

namespace Modix.Web.Server.Authorization
{
    internal static partial class AuthorizationLogMessages
    {
        public static void GrantedPermissionIdsRetrievalFailed(
                ILogger         logger,
                Snowflake       userId,
                Snowflake       guildId,
                IResultError    error)
            => GrantedPermissionIdsRetrievalFailed(
                logger,
                userId,
                guildId,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);

        [LoggerMessage(
            EventId = 0x06A767DC,
            Level   = LogLevel.Error,
            Message = "Granted permission IDs retrieval failed: {ErrorType}: {ErrorMessage}")]
        private static partial void GrantedPermissionIdsRetrievalFailed(
            ILogger     logger,
            Snowflake   userId,
            Snowflake   guildId,
            string      errorType,
            string      errorMessage,
            Exception?  exception);

        [LoggerMessage(
            EventId = 0x6D6B5D49,
            Level   = LogLevel.Debug,
            Message = "Granted permission IDs Retrieved: (UserId {UserId}, GuildId {GuildId}, {PermissionCount} permissions)")]
        public static partial void GrantedPermissionIdsRetrieved(
            ILogger     logger,
            Snowflake   userId,
            Snowflake   guildId,
            int         permissionCount);

        [LoggerMessage(
            EventId = 0x0E275801,
            Level   = LogLevel.Debug,
            Message = "Retrieving granted permission IDs: (UserId {UserId}, GuildId {GuildId})")]
        public static partial void GrantedPermissionIdsRetrieving(
            ILogger     logger,
            Snowflake   userId,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x27E292A6,
            Level   = LogLevel.Warning,
            Message = "Guild ID was invalid: {RawValue}")]
        public static partial void GuildIdInvalid(
            ILogger logger,
            string  rawValue);

        [LoggerMessage(
            EventId = 0x13E4EBA7,
            Level   = LogLevel.Warning,
            Message = "Guild ID not found")]
        public static partial void GuildIdNotFound(ILogger logger);

        [LoggerMessage(
            EventId = 0x3B513694,
            Level   = LogLevel.Debug,
            Message = "Guild ID retrieved ({GuildId})")]
        public static partial void GuildIdRetrieved(
            ILogger     logger,
            Snowflake   guildId);

        [LoggerMessage(
            EventId = 0x343621BA,
            Level   = LogLevel.Debug,
            Message = "Retrieving guild ID")]
        public static partial void GuildIdRetrieving(ILogger logger);

        public static void PermissionRequirementHandling(
                ILogger                 logger,
                PermissionRequirement   requirement)
            => PermissionRequirementHandling(
                logger,
                requirement.PermissionId,
                requirement.PermissionCategory,
                requirement.PermissionName);

        [LoggerMessage(
            EventId = 0x226AD6C2,
            Level   = LogLevel.Debug,
            Message = "Handling permission requirement: {PermissionCategory}.{PermissionName} ({PermissionId})")]
        private static partial void PermissionRequirementHandling(
            ILogger logger,
            int     permissionId,
            string  permissionCategory,
            string  permissionName);

        public static void PermissionRequirementNotSatisfied(
                ILogger                 logger,
                PermissionRequirement   requirement)
            => PermissionRequirementNotSatisfied(
                logger,
                requirement.PermissionId,
                requirement.PermissionCategory,
                requirement.PermissionName);

        [LoggerMessage(
            EventId = 0x1CB6778D,
            Level   = LogLevel.Warning,
            Message = "Permission requirement not satisfied: {PermissionCategory}.{PermissionName} ({PermissionId})")]
        private static partial void PermissionRequirementNotSatisfied(
            ILogger logger,
            int     permissionId,
            string  permissionCategory,
            string  permissionName);

        public static void PermissionRequirementSatisfied(
                ILogger                 logger,
                PermissionRequirement   requirement)
            => PermissionRequirementSatisfied(
                logger,
                requirement.PermissionId,
                requirement.PermissionCategory,
                requirement.PermissionName);

        [LoggerMessage(
            EventId = 0x32053CDD,
            Level   = LogLevel.Debug,
            Message = "Permission requirement satisfied: {PermissionCategory}.{PermissionName} ({PermissionId})")]
        private static partial void PermissionRequirementSatisfied(
            ILogger logger,
            int     permissionId,
            string  permissionCategory,
            string  permissionName);

        [LoggerMessage(
            EventId = 0x5D9285FF,
            Level   = LogLevel.Warning,
            Message = "User ID was invalid: {RawValue}")]
        public static partial void UserIdInvalid(
            ILogger logger,
            string  rawValue);

        [LoggerMessage(
            EventId = 0x7DE7EE6D,
            Level   = LogLevel.Warning,
            Message = "User ID not found")]
        public static partial void UserIdNotFound(ILogger logger);

        [LoggerMessage(
            EventId = 0x0BF343D7,
            Level   = LogLevel.Debug,
            Message = "User ID retrieved ({UserId})")]
        public static partial void UserIdRetrieved(
            ILogger     logger,
            Snowflake   userId);

        [LoggerMessage(
            EventId = 0x7A6C6D59,
            Level   = LogLevel.Debug,
            Message = "Retrieving user ID")]
        public static partial void UserIdRetrieving(ILogger logger);
    }
}
