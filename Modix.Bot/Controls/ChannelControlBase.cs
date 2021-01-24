using System;
using System.Reactive.Linq;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public abstract class ChannelControlBase
        : ControlBase
    {
        protected ChannelControlBase(
                Snowflake? guildId,
                Snowflake channelId,
                IObservable<IGuildDelete?> guildDeleted,
                IObservable<IChannelDelete?> channelDeleted,
                IObservable<ControlException> hostDeleted)
            : base(
                guildId:        guildId,
                guildDeleted:   guildDeleted,
                hostDeleted:    Observable.Merge(
                    hostDeleted,
                    channelDeleted
                        .WhereNotNull()
                        .Where(@event => @event.ID == channelId)
                        .Select(@event => new ControlException("The channel hosting this control was deleted"))))
        { }
    }
}
