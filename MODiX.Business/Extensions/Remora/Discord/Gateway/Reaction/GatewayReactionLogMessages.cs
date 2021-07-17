using System;

using Microsoft.Extensions.Logging;

using Modix.Business;

namespace Remora.Discord.Gateway.Reaction
{
    internal static class GatewayReactionLogMessages
    {
        private enum EventType
        {
            EventDispatching    = BusinessLogEventType.GatewayReaction + 0x0100,
            EventDispatched     = BusinessLogEventType.GatewayReaction + 0x0200
        }

        public static void EventDispatched<TGatewayEvent>(ILogger logger)
            => _eventDispatched.Invoke(
                logger,
                typeof(TGatewayEvent).Name);
        private static readonly Action<ILogger, string> _eventDispatched
            = LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    EventType.EventDispatched.ToEventId(),
                    "Gateway event dispatched ({EventType})")
                .WithoutException();

        public static void EventDispatching<TGatewayEvent>(ILogger logger)
            => _eventDispatching.Invoke(
                logger,
                typeof(TGatewayEvent).Name);
        private static readonly Action<ILogger, string> _eventDispatching
            = LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    EventType.EventDispatching.ToEventId(),
                    "Dispatching gateway event ({EventType})")
                .WithoutException();
    }
}
