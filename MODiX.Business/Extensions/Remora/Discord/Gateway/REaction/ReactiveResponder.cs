using System;
using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Remora.Discord.Gateway.Reaction
{
    public class ReactiveResponder<TGatewayEvent>
            : IResponder<TGatewayEvent>
        where TGatewayEvent : IGatewayEvent
    {
        public ReactiveResponder(IObserver<TGatewayEvent> observer)
            => _observer = observer;

        public Task<Result> RespondAsync(TGatewayEvent gatewayEvent, CancellationToken ct = default)
        {
            _observer.OnNext(gatewayEvent);

            return Task.FromResult(Result.FromSuccess());
        }

        private readonly IObserver<TGatewayEvent> _observer;
    }
}
