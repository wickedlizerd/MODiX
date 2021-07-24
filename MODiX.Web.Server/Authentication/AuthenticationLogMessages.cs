using System;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Core;
using Remora.Results;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Server.Authentication
{
    internal static partial class AuthenticationLogMessages
    {
        public static void LoginCompleted(
                ILogger         logger,
                LoginSuccess    response)
            => LoginCompleted(
                logger,
                response.Ticket.UserId,
                response.Ticket.Created,
                response.Ticket.Expires);

        [LoggerMessage(
            EventId = 0x558A8B8C,
            Level   = LogLevel.Debug,
            Message = "Login sequence completed (UserId: {UserId})")]
        private static partial void LoginCompleted(
            ILogger         logger,
            ulong           userId,
            DateTimeOffset  ticketCreated,
            DateTimeOffset  ticketExpires);

        public static void LoginCompleting(
                ILogger                 logger,
                CompleteLoginRequest    request)
            => LoginCompleting(
                logger,
                request.RedirectUrl);

        [LoggerMessage(
            EventId = 0x7A09ECE1,
            Level   = LogLevel.Debug,
            Message = "Completing login sequence")]
        private static partial void LoginCompleting(
            ILogger logger,
            string  redirectUrl);

        public static void LoginStarting(
                ILogger             logger,
                StartLoginRequest   request)
            => LoginStarting(
                logger,
                request.RedirectUri,
                request.State);

        [LoggerMessage(
            EventId = 0x33838087,
            Level   = LogLevel.Debug,
            Message = "Starting login sequence")]
        private static partial void LoginStarting(
            ILogger logger,
            string  redirectUrl,
            string? state);

        public static void LoginStarted(
                ILogger             logger,
                StartLoginResponse  response)
            => LoginStarted(
                logger,
                response.AuthorizeUri);

        [LoggerMessage(
            EventId = 0x1CC6CD91,
            Level   = LogLevel.Debug,
            Message = "Login sequence started")]
        private static partial void LoginStarted(
            ILogger logger,
            string  authorizeUrl);

        public static void TokenGrantAcquired(
                ILogger         logger,
                OAuthTokenGrant tokenGrant)
            => TokenGrantAcquired(
                logger,
                tokenGrant.TokenType,
                tokenGrant.Scope,
                tokenGrant.ExpiresIn);

        [LoggerMessage(
            EventId = 0x000E5D67,
            Level   = LogLevel.Debug,
            Message = "Token grant acquired")]
        private static partial void TokenGrantAcquired(
            ILogger logger,
            string  tokenType,
            string  scope,
            int     expiresIn);

        [LoggerMessage(
            EventId = 0x753DE350,
            Level   = LogLevel.Debug,
            Message = "Acquiring token grant")]
        public static partial void TokenGrantAcquiring(ILogger logger);

        public static void TokenGrantAcquisitionFailed(
                ILogger         logger,
                IResultError    error)
            => TokenGrantAcquisitionFailed(
                logger,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);

        [LoggerMessage(
            EventId = 0x4CB95003,
            Level   = LogLevel.Error,
            Message = "Token grant acquisition failed: {ErrorType}: {ErrorMessage}")]
        private static partial void TokenGrantAcquisitionFailed(
            ILogger     logger,
            string      errorType,
            string      errorMessage,
            Exception?  exception);

        [LoggerMessage(
            EventId = 0x64C0B480,
            Level   = LogLevel.Warning,
            Message = "An unexpected exception occurred while revoking tokens")]
        public static partial void TokenRevocationEncounteredException(
            ILogger     logger,
            Exception   exception);

        public static void TokenRevocationFailed(
                ILogger         logger,
                string          tokenTypeHint,
                IResultError    error)
            => TokenRevocationFailed(
                logger,
                tokenTypeHint,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);

        [LoggerMessage(
            EventId = 0x3E819DBD,
            Level   = LogLevel.Warning,
            Message = "Revocation of {TokenTypeHint} failed: {ErrorType}: {ErrorMessage}")]
        private static partial void TokenRevocationFailed(
            ILogger     logger,
            string      tokenTypeHint,
            string      errorType,
            string      errorMessage,
            Exception?  exception);

        [LoggerMessage(
            EventId = 0x4D47EE99,
            Level   = LogLevel.Debug,
            Message = "{TokenTypeHint} revoked")]
        public static partial void TokenRevoked(
            ILogger logger,
            string  tokenTypeHint);

        [LoggerMessage(
            EventId = 0x1CC07772,
            Level   = LogLevel.Debug,
            Message = "Revoking {TokenTypeHint}")]
        public static partial void TokenRevoking(
            ILogger logger,
            string  tokenTypeHint);

        public static void UserRetrievalFailed(
                ILogger         logger,
                IResultError    error)
            => UserRetrievalFailed(
                logger,
                error.GetType().Name,
                error.Message,
                (error as ExceptionError)?.Exception);

        [LoggerMessage(
            EventId = 0x53385A3A,
            Level   = LogLevel.Error,
            Message = "User info retrieval failed: {ErrorType}: {ErrorMessage}")]
        private static partial void UserRetrievalFailed(
            ILogger     logger,
            string      errorType,
            string      errorMessage,
            Exception?  exception);

        public static void UserRetrieved(
                ILogger logger,
                IUser   user)
            => UserRetrieved(
                logger,
                user.ID,
                user.Username,
                user.Discriminator);

        [LoggerMessage(
            EventId = 0x4854FE9B,
            Level   = LogLevel.Debug,
            Message = "User info retrieved: ({UserId}, {Username}#{Discriminator})")]
        private static partial void UserRetrieved(
            ILogger     logger,
            Snowflake   userId,
            string      username,
            ushort      discriminator);

        [LoggerMessage(
            EventId = 0x5C38594A,
            Level   = LogLevel.Debug,
            Message = "Retrieving user info")]
        public static partial void UserRetrieving(ILogger logger);
    }
}
