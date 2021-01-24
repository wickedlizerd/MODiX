using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public sealed class ReactionButton
        : MessageControlBase
    {
        public static async Task<ReactionButton> CreateAsync(
            IDiscordRestChannelAPI channelApi,
            IDiscordRestUserAPI userApi,
            IObservable<IGuildDelete?> guildDeleted,
            IObservable<IChannelDelete?> channelDeleted,
            IObservable<IMessageDelete?> messageDeleted,
            IObservable<IMessageReactionAdd?> messageReactionAdded,
            IObservable<IMessageReactionRemove?> messageReactionRemoved,
            Snowflake? guildId,
            Snowflake channelId,
            Snowflake messageId,
            string emojiName)
        {
            var getCurrentUserResult = await userApi.GetCurrentUserAsync();
            if (!getCurrentUserResult.IsSuccess)
                throw new ControlException($"Uncable to create button: {getCurrentUserResult.ErrorReason}", getCurrentUserResult.Exception);

            var createReactionResult = await channelApi.CreateReactionAsync(channelId, messageId, emojiName);
            return !createReactionResult.IsSuccess
                ? throw new ControlException($"Unable to create button: {createReactionResult.ErrorReason}", createReactionResult.Exception)
                : new ReactionButton(
                    guildId:                guildId,
                    channelId:              channelId,
                    messageId:              messageId,
                    selfId:                 getCurrentUserResult.Entity.ID,
                    emojiName:              emojiName,
                    guildDeleted:           guildDeleted,
                    channelDeleted:         channelDeleted,
                    messageDeleted:         messageDeleted,
                    channelApi:             channelApi,
                    messageReactionAdded:   messageReactionAdded,
                    messageReactionRemoved: messageReactionRemoved);
        }

        private ReactionButton(
                Snowflake? guildId,
                Snowflake channelId,
                Snowflake messageId,
                Snowflake selfId,
                string emojiName,
                IObservable<IGuildDelete?> guildDeleted,
                IObservable<IChannelDelete?> channelDeleted,
                IObservable<IMessageDelete?> messageDeleted,
                IDiscordRestChannelAPI channelApi,
                IObservable<IMessageReactionAdd?> messageReactionAdded,
                IObservable<IMessageReactionRemove?> messageReactionRemoved)
            : base(
                guildId:        guildId,
                channelId:      channelId,
                messageId:      messageId,
                guildDeleted:   guildDeleted,
                channelDeleted: channelDeleted,
                messageDeleted: messageDeleted,
                hostDeleted:    Observable.Empty<ControlException>())
        {
            _channelApi = channelApi;

            _clicked = Observable.Merge(
                    HostDeleted.Throw<ReactionButtonClickedEvent>(),
                    Observable.Merge(
                            messageReactionAdded
                                .WhereNotNull()
                                .Select(@event => (@event.MessageID, @event.UserID, @event.Emoji)),
                            messageReactionRemoved
                                .WhereNotNull()
                                .Select(@event => (@event.MessageID, @event.UserID, @event.Emoji)))
                        .Where(@event => (@event.MessageID == messageId)
                            && (@event.UserID != selfId)
                            && @event.Emoji.Name.HasValue
                            && (@event.Emoji.Name.Value == emojiName))
                        .Select(@event => new ReactionButtonClickedEvent(
                            emojiName:  emojiName,
                            clickedBy:  @event.UserID)))
                .Share();
        }

        public IObservable<ReactionButtonClickedEvent> Clicked
            => _clicked;

        protected override async ValueTask OnDisposingAsync(DisposalType type)
        {
            if (!IsHostDeleted)
                await _channelApi.DeleteAllReactionsAsync(ChannelId, MessageId);

            await base.OnDisposingAsync(type);
        }

        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly IObservable<ReactionButtonClickedEvent> _clicked;
    }
}
