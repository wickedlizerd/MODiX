using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Core;
using Remora.Results;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Server.Authentication
{
    internal static class AuthenticationLogMessages
    {
        private enum EventType
        {
            LoginStarting                       = ServerLogEventType.Authentication + 0x0100,
            LoginStarted                        = ServerLogEventType.Authentication + 0x0200,
            LoginCompleting                     = ServerLogEventType.Authentication + 0x0300,
            LoginCompleted                      = ServerLogEventType.Authentication + 0x0400,
            TokenGrantAcquiring                 = ServerLogEventType.Authentication + 0x0500,
            TokenGrantAcquisitionFailed         = ServerLogEventType.Authentication + 0x0600,
            TokenGrantAcquired                  = ServerLogEventType.Authentication + 0x0700,
            UserRetrieving                      = ServerLogEventType.Authentication + 0x0800,
            UserRetrievalFailed                 = ServerLogEventType.Authentication + 0x0900,
            UserRetrieved                       = ServerLogEventType.Authentication + 0x0A00,
            TokenRevoking                       = ServerLogEventType.Authentication + 0x0B00,
            TokenRevocationFailed               = ServerLogEventType.Authentication + 0x0C00,
            TokenRevoked                        = ServerLogEventType.Authentication + 0x0D00,
            TokenRevocationEncounteredException = ServerLogEventType.Authentication + 0x0E00
        }

        public static void LoginCompleted(
                ILogger         logger,
                LoginSuccess    response)
            => _loginCompleted.Invoke(
                logger,
                response.Ticket.UserId,
                response.Ticket.Created,
                response.Ticket.Expires);
        private static readonly Action<ILogger, ulong, DateTimeOffset, DateTimeOffset> _loginCompleted
            = StructuredLoggerMessage.Define<ulong, DateTimeOffset, DateTimeOffset>(
                    LogLevel.Debug,
                    EventType.LoginCompleted.ToEventId(),
                    "Login sequence completed (UserId: {UserId})",
                    "TicketCreated",
                    "TicketExpires")
                .WithoutException();

        public static void LoginCompleting(
                ILogger                 logger,
                CompleteLoginRequest    request)
            => _loginCompleting.Invoke(
                logger,
                request.RedirectUrl);
        private static readonly Action<ILogger, string> _loginCompleting
            = StructuredLoggerMessage.Define<string>(
                    LogLevel.Debug,
                    EventType.LoginCompleted.ToEventId(),
                    "Completing login sequence",
                    "RedirectUrl")
                .WithoutException();

        public static void LoginStarting(
                ILogger             logger,
                StartLoginRequest   request)
            => _loginStarting.Invoke(
                logger,
                request.RedirectUri,
                request.State);
        private static readonly Action<ILogger, string, string?> _loginStarting
            = StructuredLoggerMessage.Define<string, string?>(
                    LogLevel.Debug,
                    EventType.LoginStarting.ToEventId(),
                    "Starting login sequence",
                    "RedirectUri",
                    "State")
                .WithoutException();

        public static void LoginStarted(
                ILogger             logger,
                StartLoginResponse  response)
            => _loginStarted.Invoke(
                logger,
                response.AuthorizeUri);
        private static readonly Action<ILogger, string> _loginStarted
            = StructuredLoggerMessage.Define<string>(
                    LogLevel.Debug,
                    EventType.LoginStarted.ToEventId(),
                    "Login sequence started",
                    "AuthorizeUri")
                .WithoutException();

        public static void TokenGrantAcquired(
                ILogger         logger,
                OAuthTokenGrant tokenGrant)
            => _tokenGrantAcquired.Invoke(
                logger,
                tokenGrant.TokenType,
                tokenGrant.Scope,
                tokenGrant.ExpiresIn);
        private static readonly Action<ILogger, string, string, int> _tokenGrantAcquired
            = StructuredLoggerMessage.Define<string, string, int>(
                    LogLevel.Debug,
                    EventType.TokenGrantAcquired.ToEventId(),
                    "Token grant acquired",
                    "TokenType",
                    "Scope",
                    "ExpiresIn")
                .WithoutException();

        public static void TokenGrantAcquiring(ILogger logger)
            => _tokenGrantAcquiring.Invoke(logger);
        private static readonly Action<ILogger> _tokenGrantAcquiring
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.TokenGrantAcquiring.ToEventId(),
                    "Acquiring token grant")
                .WithoutException();

        public static void TokenGrantAcquisitionFailed(
                ILogger         logger,
                IResultError    error)
            => _tokenGrantAcquisitionFailed.Invoke(
                logger,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);
        private static readonly Action<ILogger, string, string, Exception?> _tokenGrantAcquisitionFailed
            = LoggerMessage.Define<string, string>(
                LogLevel.Error,
                EventType.TokenGrantAcquisitionFailed.ToEventId(),
                "Token grant acquisition failed: {ErrorType}: {ErrorMessage}");

        public static void TokenRevocationEncounteredException(
                ILogger     logger,
                Exception   exception)
            => _tokenRevocationEncounteredException.Invoke(
                logger,
                exception);
        private static readonly Action<ILogger, Exception?> _tokenRevocationEncounteredException
            = LoggerMessage.Define(
                LogLevel.Warning,
                EventType.TokenRevocationEncounteredException.ToEventId(),
                "An unexpected exception occurred while revoking tokens");

        public static void TokenRevocationFailed(
                ILogger         logger,
                string          tokenTypeHint,
                IResultError    error)
            => _tokenRevocationFailed.Invoke(
                logger,
                tokenTypeHint,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);
        private static readonly Action<ILogger, string, string, string, Exception?> _tokenRevocationFailed
            = LoggerMessage.Define<string, string, string>(
                LogLevel.Warning,
                EventType.TokenRevocationFailed.ToEventId(),
                "Revocation of {TokenTypeHint} failed: {ErrorType}: {ErrorMessage}");

        public static void TokenRevoked(
                ILogger         logger,
                string          tokenTypeHint)
            => _tokenRevoked.Invoke(
                logger,
                tokenTypeHint);
        private static readonly Action<ILogger, string> _tokenRevoked
            = LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    EventType.TokenRevoked.ToEventId(),
                    "{TokenTypeHint} revoked")
                .WithoutException();

        public static void TokenRevoking(
                ILogger         logger,
                string          tokenTypeHint)
            => _tokenRevoking.Invoke(
                logger,
                tokenTypeHint);
        private static readonly Action<ILogger, string> _tokenRevoking
            = LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    EventType.TokenRevoking.ToEventId(),
                    "Revoking {TokenTypeHint}")
                .WithoutException();

        public static void UserRetrievalFailed(
                ILogger         logger,
                IResultError    error)
            => _userRetrievalFailed.Invoke(
                logger,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);
        private static readonly Action<ILogger, string, string, Exception?> _userRetrievalFailed
            = LoggerMessage.Define<string, string>(
                LogLevel.Error,
                EventType.UserRetrievalFailed.ToEventId(),
                "User info retrieval failed: {ErrorType}: {ErrorMessage}");

        public static void UserRetrieved(
                ILogger logger,
                IUser   user)
            => _userRetrieved.Invoke(
                logger,
                user.ID,
                user.Username,
                user.Discriminator);
        private static readonly Action<ILogger, Snowflake, string, ushort> _userRetrieved
            = LoggerMessage.Define<Snowflake, string, ushort>(
                    LogLevel.Debug,
                    EventType.UserRetrieved.ToEventId(),
                    "User info retrieved: ({UserId}, {Username}#{Discriminator})")
                .WithoutException();

        public static void UserRetrieving(ILogger logger)
            => _userRetrieving.Invoke(logger);
        private static readonly Action<ILogger> _userRetrieving
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.UserRetrieving.ToEventId(),
                    "Retrieving user info")
                .WithoutException();
    }
}
