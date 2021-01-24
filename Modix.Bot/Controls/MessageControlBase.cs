using System;
using System.Linq;
using System.Reactive.Linq;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public class MessageControlBase
        : ChannelControlBase
    {
        protected MessageControlBase(
                Snowflake? guildId,
                Snowflake channelId,
                Snowflake messageId,
                IObservable<IGuildDelete?> guildDeleted,
                IObservable<IChannelDelete?> channelDeleted,
                IObservable<IMessageDelete?> messageDeleted,
                IObservable<ControlException> hostDeleted)
            : base(
                guildId:            guildId,
                channelId:          channelId,
                guildDeleted:       guildDeleted,
                channelDeleted:     channelDeleted,
                hostDeleted:        Observable.Merge(
                    hostDeleted,
                    messageDeleted
                        .WhereNotNull()
                        .Where(@event => @event.ID == messageId)
                        .Select(@event => new ControlException("The message hosting this control was deleted"))))
        { }
    }
}
