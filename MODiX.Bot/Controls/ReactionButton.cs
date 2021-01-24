using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public class ReactionButton
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
            string emojiName,
            CancellationToken cancellationToken)
        {
            var getCurrentUserResult = await userApi.GetCurrentUserAsync(cancellationToken);
            if (!getCurrentUserResult.IsSuccess)
                throw new ControlException($"Uncable to create button: {getCurrentUserResult.ErrorReason}", getCurrentUserResult.Exception);

            var createReactionResult = await channelApi.CreateReactionAsync(channelId, messageId, emojiName, cancellationToken);
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
                guildId,
                channelId,
                messageId,
                guildDeleted,
                channelDeleted,
                messageDeleted,
                Observable.Empty<ControlException>())
        {
            _channelId = channelId;
            _messageId = messageId;

            _channelApi = channelApi;

            _clickedBy = Observable.Merge(
                HostDeleted.Throw<Snowflake>(),
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
                    .Select(@event => @event.UserID));
        }

        public IObservable<Snowflake> ClickedBy
            => _clickedBy;

        protected override async ValueTask OnDisposingAsync(DisposalType type)
        {
            if (!IsHostDeleted)
                await _channelApi.DeleteAllReactionsAsync(_channelId, _messageId);

            await base.OnDisposingAsync(type);
        }

        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly Snowflake _channelId;
        private readonly IObservable<Snowflake> _clickedBy;
        private readonly Snowflake _messageId;
    }
}
