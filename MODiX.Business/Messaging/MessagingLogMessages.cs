using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.API.Abstractions.Gateway.Events;

namespace Modix.Business.Messaging
{
    internal static partial class MessagingLogMessages
    {
        public static IDisposable BeginHandlerScope<TNotification>(
                    ILogger                             logger,
                    INotificationHandler<TNotification> handler)
                where TNotification : notnull
            => _beginHandlerScope.Invoke(
                logger,
                handler.GetType().Name);
        private static readonly Func<ILogger, string, IDisposable> _beginHandlerScope
            = StructuredLoggerMessage.DefineScopeData<string>("NotificationHandlerType");

        public static IDisposable BeginNotificationScope<TNotification>(
                    ILogger logger,
                    Guid    scopeId)
                where TNotification : notnull
            => _beginNotificationScope.Invoke(
                logger,
                typeof(TNotification).Name,
                scopeId);
        private static readonly Func<ILogger, string, Guid, IDisposable> _beginNotificationScope
            = StructuredLoggerMessage.DefineScopeData<string, Guid>(
                "NotificationType",
                "ScopeId");

        [LoggerMessage(
            EventId = 0x0D2E1473,
            Level   = LogLevel.Error,
            Message = "A notification handler threw an exception.")]
        public static partial void HandlerInvocationFailed(
            ILogger     logger,
            Exception   exception);

        [LoggerMessage(
            EventId = 0x53F8A2FA,
            Level   = LogLevel.Debug,
            Message = "Notification handler invoked")]
        public static partial void HandlerInvoked(ILogger logger);

        [LoggerMessage(
            EventId = 0x7EE0F5CA,
            Level   = LogLevel.Debug,
            Message = "Invoking notification handler")]
        public static partial void HandlerInvoking(ILogger logger);

        [LoggerMessage(
            EventId = 0x62DF1207,
            Level   = LogLevel.Debug,
            Message = "Notification handlers invoked")]
        public static partial void HandlersInvoked(ILogger logger);

        [LoggerMessage(
            EventId = 0x1B19A2A4,
            Level   = LogLevel.Debug,
            Message = "Invoking notification handlers")]
        public static partial void HandlersInvoking(ILogger logger);

        [LoggerMessage(
            EventId = 0x3B5BCB5D,
            Level   = LogLevel.Error,
            Message = "An unknown event was received on the Discord Gateway")]
        public static partial void UnknownEventReceived(
            ILogger         logger,
            IUnknownEvent   unknownEvent);
    }
}
