﻿using System;
using System.Reactive.Subjects;

using Microsoft.Extensions.DependencyInjection;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Extensions;

namespace Remora.Discord.Gateway.Reaction
{
    public static class Setup
    {
        public static IServiceCollection AddGatewayReaction(this IServiceCollection services)
            => services
                .AddGatewayReaction<IChannelDelete>()
                .AddGatewayReaction<IChannelUpdate>()
                .AddGatewayReaction<IGuildCreate>()
                .AddGatewayReaction<IGuildDelete>()
                .AddGatewayReaction<IGuildUpdate>()
                .AddGatewayReaction<IGuildMemberAdd>()
                .AddGatewayReaction<IGuildMemberUpdate>()
                .AddGatewayReaction<IMessageCreate>()
                .AddGatewayReaction<IMessageDelete>()
                .AddGatewayReaction<IMessageReactionAdd>()
                .AddGatewayReaction<IMessageReactionRemove>()
                .AddGatewayReaction<IMessageUpdate>()
                .AddGatewayReaction<IPresenceUpdate>();

        public static IServiceCollection AddGatewayReaction<TGatewayEvent>(this IServiceCollection services)
                where TGatewayEvent : IGatewayEvent
            => services
                .AddSingleton<Subject<TGatewayEvent>>()
                .AddSingleton<IObserver<TGatewayEvent>, Subject<TGatewayEvent>>(serviceProvider => serviceProvider.GetRequiredService<Subject<TGatewayEvent>>())
                .AddSingleton<IObservable<TGatewayEvent>, Subject<TGatewayEvent>>(serviceProvider => serviceProvider.GetRequiredService<Subject<TGatewayEvent>>())
                .AddResponder<ReactiveResponder<TGatewayEvent>>();
    }
}
