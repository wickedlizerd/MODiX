using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Results;
using Remora.Results;

namespace Modix.Bot.Responders
{
    public interface IResponseDelegator<TGatewayEvent>
        where TGatewayEvent : IGatewayEvent
    {
        Task<EventResponseResult> OnRespondingAsync(TGatewayEvent? gatewayEvent, CancellationToken cancellationToken);

        IDisposable RespondWith(ResponseDelegate<TGatewayEvent> respondAsync);
    }

    public class ResponseDelegator<TGatewayEvent>
            : IResponseDelegator<TGatewayEvent>
        where TGatewayEvent : IGatewayEvent
    {
        public ResponseDelegator()
        {
            _responseDelegatesBySubscription = new();
        }

        public async Task<EventResponseResult> OnRespondingAsync(TGatewayEvent? gatewayEvent, CancellationToken cancellationToken)
        {
            var results = await Task.WhenAll(_responseDelegatesBySubscription
                .Select(pair => pair.Value.Invoke(gatewayEvent, cancellationToken)));

            var failedResults = results
                .Where(result => !result.IsSuccess);

            return failedResults.Any()
                ? EventResponseResult.FromError(AggregateResult.FromResults(failedResults))
                : EventResponseResult.FromSuccess();
        }

        public IDisposable RespondWith(ResponseDelegate<TGatewayEvent> onRespondingAsync)
        {
            var susbcription = new DelegateDisposable(@this => _responseDelegatesBySubscription.TryRemove(@this, out var _));

            _responseDelegatesBySubscription.TryAdd(susbcription, onRespondingAsync);

            return susbcription;
        }

        private readonly ConcurrentDictionary<IDisposable, ResponseDelegate<TGatewayEvent>> _responseDelegatesBySubscription;
    }
}
