using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;

using Remora.Discord.API.Abstractions.Gateway.Events;

namespace Modix.Business.Messaging
{
    internal static class MessagingLogMessages
    {
        private enum EventType
        {
            HandlersInvoking        = BusinessLogEventType.Messaging + 0x0100,
            HandlersInvoked         = BusinessLogEventType.Messaging + 0x0200,
            HandlerInvoking         = BusinessLogEventType.Messaging + 0x0300,
            HandlerInvoked          = BusinessLogEventType.Messaging + 0x0400,
            HandlerInvocationFailed = BusinessLogEventType.Messaging + 0x0500,
            UnknownEventReceived    = BusinessLogEventType.Messaging + 0x0600
        }

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

        public static void HandlerInvocationFailed(
                ILogger     logger,
                Exception   exception)
            => _handlerInvocationFailed.Invoke(
                logger,
                exception);
        private static readonly Action<ILogger, Exception> _handlerInvocationFailed
            = LoggerMessage.Define(
                LogLevel.Error,
                EventType.HandlerInvocationFailed.ToEventId(),
                "A notification handler threw an exception.");

        public static void HandlerInvoked(ILogger logger)
            => _handlerInvoked.Invoke(logger);
        private static readonly Action<ILogger> _handlerInvoked
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.HandlerInvoked.ToEventId(),
                    "Notification handler invoked")
                .WithoutException();

        public static void HandlerInvoking(ILogger logger)
            => _handlerInvoking.Invoke(logger);
        private static readonly Action<ILogger> _handlerInvoking
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.HandlerInvoking.ToEventId(),
                    "Invoking notification handler")
                .WithoutException();

        public static void HandlersInvoked(ILogger logger)
            => _handlersInvoked.Invoke(logger);
        private static readonly Action<ILogger> _handlersInvoked
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.HandlersInvoked.ToEventId(),
                    "Notification handlers invoked")
                .WithoutException();

        public static void HandlersInvoking(ILogger logger)
            => _handlersInvoking.Invoke(logger);
        private static readonly Action<ILogger> _handlersInvoking
            = LoggerMessage.Define(
                    LogLevel.Debug,
                    EventType.HandlersInvoking.ToEventId(),
                    "Invoking notification handlers")
                .WithoutException();

        public static void UnknownEventReceived(
                ILogger         logger,
                IUnknownEvent   unknownEvent)
            => _unknownEventReceived.Invoke(
                logger,
                unknownEvent.Data);
        private static readonly Action<ILogger, string> _unknownEventReceived
            = StructuredLoggerMessage.Define<string>(
                    LogLevel.Error,
                    EventType.UnknownEventReceived.ToEventId(),
                    "An unknown event was received on the Discord Gateway",
                    "EventData")
                .WithoutException();

        private struct StructuredLoggerState<T1>
            : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public StructuredLoggerState(KeyValuePair<string, T1> pair1)
                => _pair1 = pair1;

            public KeyValuePair<string, object?> this[int index]
                => index switch
                {
                    0 => new(_pair1.Key, _pair1.Value),
                    _ => throw new IndexOutOfRangeException()
                };

            public int Count
                => 1;

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                yield return new(_pair1.Key, _pair1.Value);
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            public override string ToString()
                => $"{_pair1.Key}: {_pair1.Value}";

            private readonly KeyValuePair<string, T1> _pair1;
        }

        private struct StructuredLoggerState<T1, T2>
            : IReadOnlyList<KeyValuePair<string, object?>>
        {
            public StructuredLoggerState(
                KeyValuePair<string, T1> pair1,
                KeyValuePair<string, T2> pair2)
            {
                _pair1 = pair1;
                _pair2 = pair2;
            }

            public KeyValuePair<string, object?> this[int index]
                => index switch
                {
                    0 => new(_pair1.Key, _pair1.Value),
                    1 => new(_pair2.Key, _pair2.Value),
                    _ => throw new IndexOutOfRangeException()
                };

            public int Count
                => 1;

            public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            {
                yield return new(_pair1.Key, _pair1.Value);
                yield return new(_pair2.Key, _pair2.Value);
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            public override string ToString()
                => $"{_pair1.Key}: {_pair1.Value}, {_pair2.Key}: {_pair2.Value}";

            private readonly KeyValuePair<string, T1> _pair1;
            private readonly KeyValuePair<string, T2> _pair2;
        }
    }
}
