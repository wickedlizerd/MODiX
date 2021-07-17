using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Modix.Business.Messaging
{
    public class MessagingResponder<TGatewayEvent>
            : IResponder<TGatewayEvent>
        where TGatewayEvent : IGatewayEvent
    {
        public MessagingResponder(
            IOptions<MessagingConfiguration>                    configuration,
            IEnumerable<INotificationHandler<TGatewayEvent>>    notificationHandlers,
            ILogger<MessagingResponder<TGatewayEvent>>          logger)
        {
            _configuration          = configuration;
            _notificationHandlers   = notificationHandlers;
            _logger                 = logger;
        }

        public async Task<Result> RespondAsync(
            TGatewayEvent       gatewayEvent,
            CancellationToken   cancellationToken)
        {
            using var cancellationTokenSource = new CancellationTokenSource(_configuration.Value.DispatchTimeout ?? Timeout.InfiniteTimeSpan);
            using var logScope = MessagingLogMessages.BeginNotificationScope<TGatewayEvent>(_logger, Guid.NewGuid());

            MessagingLogMessages.HandlersInvoking(_logger);
            foreach (var handler in _notificationHandlers)
            {
                using var handlerLogScope = MessagingLogMessages.BeginHandlerScope(_logger, handler);

                try
                {
                    MessagingLogMessages.HandlerInvoking(_logger);
                    await handler.HandleNotificationAsync(gatewayEvent, cancellationTokenSource.Token);
                    MessagingLogMessages.HandlerInvoked(_logger);
                }
                catch (Exception ex)
                {
                    MessagingLogMessages.HandlerInvocationFailed(_logger, ex);
                }
            }
            MessagingLogMessages.HandlersInvoked(_logger);

            return Result.FromSuccess();
        }

        private readonly IOptions<MessagingConfiguration>                   _configuration;
        private readonly IEnumerable<INotificationHandler<TGatewayEvent>>   _notificationHandlers;
        private readonly ILogger<MessagingResponder<TGatewayEvent>>         _logger;
    }
}
