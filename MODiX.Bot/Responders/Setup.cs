using Microsoft.Extensions.DependencyInjection;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Extensions;

namespace Modix.Bot.Responders
{
    public static class Setup
    {
        public static IServiceCollection AddResponders(this IServiceCollection services)
            => services
                .AddResponder<DelegateResponder<IMessageReactionAdd>>()
                .AddResponder<DelegateResponder<IMessageReactionRemove>>()
                .AddSingleton<IResponseDelegator<IMessageReactionAdd>,      ResponseDelegator<IMessageReactionAdd>>()
                .AddSingleton<IResponseDelegator<IMessageReactionRemove>,   ResponseDelegator<IMessageReactionRemove>>();
    }
}
