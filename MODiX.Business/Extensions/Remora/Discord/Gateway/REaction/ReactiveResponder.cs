using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Remora.Discord.Gateway.Reaction
{
    public class ReactiveResponder<TGatewayEvent>
            : IResponder<TGatewayEvent>
        where TGatewayEvent : IGatewayEvent
    {
        public ReactiveResponder(
            ILogger<ReactiveResponder<TGatewayEvent>>   logger,
            IObserver<TGatewayEvent>                    observer)
        {
            _logger     = logger;
            _observer   = observer;
        }

        public Task<Result> RespondAsync(TGatewayEvent gatewayEvent, CancellationToken ct = default)
        {
            GatewayReactionLogMessages.EventDispatching<TGatewayEvent>(_logger);
            _observer.OnNext(gatewayEvent);
            GatewayReactionLogMessages.EventDispatched<TGatewayEvent>(_logger);

            return Task.FromResult(Result.FromSuccess());
        }

        private readonly ILogger                    _logger;
        private readonly IObserver<TGatewayEvent>   _observer;
    }
}
