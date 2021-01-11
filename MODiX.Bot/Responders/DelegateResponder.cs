using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Discord.Gateway.Results;

namespace Modix.Bot.Responders
{
    public class DelegateResponder<TGatewayEvent>
            : IResponder<TGatewayEvent>
        where TGatewayEvent : IGatewayEvent
    {
        public DelegateResponder(
            IResponseDelegator<TGatewayEvent> responseDelegator)
        {
            _responseDelegator = responseDelegator;
        }

        public Task<EventResponseResult> RespondAsync(TGatewayEvent? gatewayEvent, CancellationToken ct = default)
            => _responseDelegator.OnRespondingAsync(gatewayEvent, ct);

        private readonly IResponseDelegator<TGatewayEvent> _responseDelegator;
    }
}
