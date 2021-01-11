using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Results;

namespace Modix.Bot.Responders
{
    public delegate Task<EventResponseResult> ResponseDelegate<TGatewayEvent>(TGatewayEvent? gatewayEvent, CancellationToken cancellationToken)
        where TGatewayEvent : IGatewayEvent;
}
