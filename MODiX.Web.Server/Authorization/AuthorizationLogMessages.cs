using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.Core;
using Remora.Results;

namespace Modix.Web.Server.Authorization
{
    internal static class AuthorizationLogMessages
    {
        private enum EventType
        {
            PermissionRequirementHandling       = ServerLogEventType.Authorization + 0x0100,
            PermissionRequirementSatisfied      = ServerLogEventType.Authorization + 0x0200,
            PermissionRequirementNotSatisfied   = ServerLogEventType.Authorization + 0x0300,
            UserIdRetrieving                    = ServerLogEventType.Authorization + 0x0400,
            UserIdNotFound                      = ServerLogEventType.Authorization + 0x0500,
            UserIdInvalid                       = ServerLogEventType.Authorization + 0x0600,
            UserIdRetrieved                     = ServerLogEventType.Authorization + 0x0700,
            GuildIdRetrieving                   = ServerLogEventType.Authorization + 0x0800,
            GuildIdNotFound                     = ServerLogEventType.Authorization + 0x0900,
            GuildIdInvalid                      = ServerLogEventType.Authorization + 0x0A00,
            GuildIdRetrieved                    = ServerLogEventType.Authorization + 0x0B00,
            GrantedPermissionIdsRetrieving      = ServerLogEventType.Authorization + 0x0C00,
            GrantedPermissionIdsRetrievalFailed = ServerLogEventType.Authorization + 0x0D00,
            GrantedPermissionIdsRetrieved       = ServerLogEventType.Authorization + 0x0E00
        }

        public static void GrantedPermissionIdsRetrievalFailed(
                ILogger         logger,
                Snowflake       userId,
                Snowflake       guildId,
                IResultError    error)
            => _grantedPermissionIdsRetrievalFailed.Invoke(
                logger,
                error.GetType().Name,
                error.Message,
                userId,
                guildId);
        private static readonly Action<ILogger, string, string, Snowflake, Snowflake> _grantedPermissionIdsRetrievalFailed
            = StructuredLoggerMessage.Define<string, string, Snowflake, Snowflake>(
                    LogLevel.Error,
                    EventType.GrantedPermissionIdsRetrievalFailed.ToEventId(),
                    "Granted permission IDs retrieval failed: {ErrorType}: {ErrorMessage}",
                    "UserId",
                    "GuildId")
                .WithoutException();

        public static void GrantedPermissionIdsRetrieved(
                ILogger     logger,
                Snowflake   userId,
                Snowflake   guildId,
                int         permissionCount)
            => _grantedPermissionIdsRetrieved.Invoke(
                logger,
                userId,
                guildId,
                permissionCount);
        private static readonly Action<ILogger, Snowflake, Snowflake, int> _grantedPermissionIdsRetrieved
            = LoggerMessage.Define<Snowflake, Snowflake, int>(
                    LogLevel.Debug,
                    EventType.GrantedPermissionIdsRetrieved.ToEventId(),
                    "Granted permission IDs Retrieved: (UserId {UserId}, GuildId {GuildId}, {PermissionCount} permissions)")
                .WithoutException();

        public static void GrantedPermissionIdsRetrieving(
                ILogger     logger,
                Snowflake   userId,
                Snowflake   guildId)
            => _grantedPermissionIdsRetrieving.Invoke(
                logger,
                userId,
                guildId);
        private static readonly Action<ILogger, Snowflake, Snowflake> _grantedPermissionIdsRetrieving
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    LogLevel.Debug,
                    EventType.GrantedPermissionIdsRetrieving.ToEventId(),
                    "Retrieving granted permission IDs: (UserId {UserId}, GuildId {GuildId})")
                .WithoutException();

        public static void GuildIdInvalid(
                ILogger logger,
                string  rawValue)
            => _guildIdInvalid(
                logger,
                rawValue);
        private static readonly Action<ILogger, string> _guildIdInvalid
            = LoggerMessage.Define<string>(
                    LogLevel.Warning,
                    EventType.GuildIdInvalid.ToEventId(),
                    "Guild ID was invalid: {RawValue}")
                .WithoutException();

        public static void GuildIdNotFound(ILogger logger)
            => _guildIdNotFound(logger);
        private static readonly Action<ILogger> _guildIdNotFound
            = LoggerMessage.Define(
                    LogLevel.Warning,
                    EventType.GuildIdNotFound.ToEventId(),
                    "Guild ID not found")
                .WithoutException();

        public static void GuildIdRetrieved(
                ILogger     logger,
                Snowflake   guildId)
            => _guildIdRetrieved(
                logger,
                guildId);
        private static readonly Action<ILogger, Snowflake> _guildIdRetrieved
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.GuildIdRetrieved.ToEventId(),
                    "Retrieving guild ID ({GuildId})")
                .WithoutException();

        public static void GuildIdRetrieving(ILogger logger)
            => _guildIdRetrieving(logger);
        private static readonly Action<ILogger> _guildIdRetrieving
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.GuildIdRetrieving.ToEventId(),
                    "Retrieving guild ID")
                .WithoutException();

        public static void PermissionRequirementHandling(
                ILogger                 logger,
                PermissionRequirement   requirement)
            => _permissionRequirementHandling.Invoke(
                logger,
                requirement.PermissionId,
                requirement.PermissionCategory,
                requirement.PermissionName);
        private static readonly Action<ILogger, int, string, string> _permissionRequirementHandling
            = LoggerMessage.Define<int, string, string>(
                    LogLevel.Debug,
                    EventType.PermissionRequirementHandling.ToEventId(),
                    "Handling permission requirement: {PermissionCategory}.{PermissionName} ({PermissionId})")
                .WithoutException();

        public static void PermissionRequirementNotSatisfied(
                ILogger                 logger,
                PermissionRequirement   requirement)
            => _permissionRequirementNotSatisfied.Invoke(
                logger,
                requirement.PermissionId,
                requirement.PermissionCategory,
                requirement.PermissionName);
        private static readonly Action<ILogger, int, string, string> _permissionRequirementNotSatisfied
            = LoggerMessage.Define<int, string, string>(
                    LogLevel.Warning,
                    EventType.PermissionRequirementNotSatisfied.ToEventId(),
                    "Permission requirement not satisfied: {PermissionCategory}.{PermissionName} ({PermissionId})")
                .WithoutException();

        public static void PermissionRequirementSatisfied(
                ILogger                 logger,
                PermissionRequirement   requirement)
            => _permissionRequirementSatisfied.Invoke(
                logger,
                requirement.PermissionId,
                requirement.PermissionCategory,
                requirement.PermissionName);
        private static readonly Action<ILogger, int, string, string> _permissionRequirementSatisfied
            = LoggerMessage.Define<int, string, string>(
                    LogLevel.Debug,
                    EventType.PermissionRequirementSatisfied.ToEventId(),
                    "Permission requirement satisfied: {PermissionCategory}.{PermissionName} ({PermissionId})")
                .WithoutException();

        public static void UserIdInvalid(
                ILogger logger,
                string  rawValue)
            => _userIdInvalid(
                logger,
                rawValue);
        private static readonly Action<ILogger, string> _userIdInvalid
            = LoggerMessage.Define<string>(
                    LogLevel.Warning,
                    EventType.UserIdInvalid.ToEventId(),
                    "User ID was invalid: {RawValue}")
                .WithoutException();

        public static void UserIdNotFound(ILogger logger)
            => _userIdNotFound(logger);
        private static readonly Action<ILogger> _userIdNotFound
            = LoggerMessage.Define(
                    LogLevel.Warning,
                    EventType.UserIdNotFound.ToEventId(),
                    "User ID not found")
                .WithoutException();

        public static void UserIdRetrieved(
                ILogger     logger,
                Snowflake   userId)
            => _userIdRetrieved(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _userIdRetrieved
            = LoggerMessage.Define<Snowflake>(
                    LogLevel.Debug,
                    EventType.UserIdRetrieved.ToEventId(),
                    "Retrieving user ID ({UserId})")
                .WithoutException();

        public static void UserIdRetrieving(ILogger logger)
            => _userIdRetrieving(logger);
        private static readonly Action<ILogger> _userIdRetrieving
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.UserIdRetrieving.ToEventId(),
                    "Retrieving user ID")
                .WithoutException();
    }
}
