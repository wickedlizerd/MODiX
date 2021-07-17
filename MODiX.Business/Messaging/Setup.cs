using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Extensions;

namespace Modix.Business.Messaging
{
    public static class Setup
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
            => services
                .Add(services => services.AddOptions<MessagingConfiguration>()
                    .Bind(configuration.GetSection("MODiX:Business:Messaging"))
                    .ValidateDataAnnotations()
                    .ValidateOnStartup())
                .AddResponder<MessagingResponder<IChannelDelete>>()
                .AddResponder<MessagingResponder<IChannelUpdate>>()
                .AddResponder<MessagingResponder<IGuildCreate>>()
                .AddResponder<MessagingResponder<IGuildDelete>>()
                .AddResponder<MessagingResponder<IGuildUpdate>>()
                .AddResponder<MessagingResponder<IGuildMemberAdd>>()
                .AddResponder<MessagingResponder<IGuildMemberUpdate>>()
                .AddResponder<MessagingResponder<IMessageCreate>>()
                .AddResponder<MessagingResponder<IMessageDelete>>()
                .AddResponder<MessagingResponder<IMessageReactionAdd>>()
                .AddResponder<MessagingResponder<IMessageReactionRemove>>()
                .AddResponder<MessagingResponder<IMessageUpdate>>()
                .AddResponder<MessagingResponder<IPresenceUpdate>>()
                .AddResponder<UnknownEventResponder>();
    }
}
