using Microsoft.Extensions.DependencyInjection;

using Remora.Discord.API.Abstractions.Gateway.Bidirectional;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Extensions;

namespace Remora.Discord.Gateway.Delegation
{
    public static class Setup
    {
        public static IServiceCollection AddGatewayDelegation(this IServiceCollection services)
            => services
                .AddResponder<DelegateResponder<IHello>>()
                .AddResponder<DelegateResponder<IHeartbeatAcknowledge>>()
                .AddResponder<DelegateResponder<IMessageReactionAdd>>()
                .AddResponder<DelegateResponder<IMessageReactionRemove>>()
                .AddSingleton<IResponseDelegator<IHello>,                   ResponseDelegator<IHello>>()
                .AddSingleton<IResponseDelegator<IHeartbeatAcknowledge>,    ResponseDelegator<IHeartbeatAcknowledge>>()
                .AddSingleton<IResponseDelegator<IMessageReactionAdd>,      ResponseDelegator<IMessageReactionAdd>>()
                .AddSingleton<IResponseDelegator<IMessageReactionRemove>,   ResponseDelegator<IMessageReactionRemove>>();
    }
}
