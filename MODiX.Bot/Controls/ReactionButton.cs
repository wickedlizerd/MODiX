using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;
using Remora.Discord.Gateway.Delegation;
using Remora.Discord.Gateway.Results;
using Remora.Results;

namespace Modix.Bot.Controls
{
    public class ReactionButton
        : ControlBase
    {
        public static async Task<ControlCreationResult<ReactionButton>> CreateAsync(
            IDiscordRestChannelAPI channelApi,
            IDiscordRestUserAPI userApi,
            IResponseDelegator<IMessageReactionAdd> reactionAddDelegator,
            IResponseDelegator<IMessageReactionRemove> reactionRemoveDelegator,
            Snowflake channelId,
            Snowflake messageId,
            string emoji,
            OperationAction onClickedAsync,
            CancellationToken cancellationToken)
        {
            var getCurrentUserResult = await userApi.GetCurrentUserAsync(cancellationToken);
            if (!getCurrentUserResult.IsSuccess)
                return ControlCreationResult<ReactionButton>.FromError(getCurrentUserResult);

            var button = new ReactionButton(
                channelApi:                 channelApi,
                channelId:                  channelId,
                currentUserId:              getCurrentUserResult.Entity.ID,
                emoji:                      emoji,
                messageId:                  messageId,
                onClickedAsync:             onClickedAsync,
                reactionAddDelegator:       reactionAddDelegator,
                reactionRemoveDelegator:    reactionRemoveDelegator);

            return await OnCreatingAsync(button, cancellationToken);
        }

        private ReactionButton(
            IDiscordRestChannelAPI channelApi,
            Snowflake channelId,
            Snowflake currentUserId,
            string emoji,
            Snowflake messageId,
            OperationAction onClickedAsync,
            IResponseDelegator<IMessageReactionAdd> reactionAddDelegator,
            IResponseDelegator<IMessageReactionRemove> reactionRemoveDelegator)
        {
            _channelApi = channelApi;
            _channelId = channelId;
            _currentUserId = currentUserId;
            _emoji = emoji;
            _messageId = messageId;
            _onClickedAsync = onClickedAsync;
            _reactionAddDelegator = reactionAddDelegator;
            _reactionRemoveDelegator = reactionRemoveDelegator;
        }

        public override async Task<OperationResult> DestroyAsync()
        {
            var deleteReactionsResult = await _channelApi.DeleteAllReactionsForEmojiAsync(_channelId, _messageId, _emoji);
            return deleteReactionsResult.IsSuccess
                ? await base.DestroyAsync()
                : OperationResult.FromError(deleteReactionsResult);
        }

        protected override async Task<OperationResult> InitializeAsync(
            CancellationToken cancellationToken)
        {
            Resources.Add(_reactionAddDelegator.RespondWith(RespondToReactionAddAsync));
            Resources.Add(_reactionRemoveDelegator.RespondWith(RespondToReactionRemoveAsync));

            var createReactionResult = await _channelApi.CreateReactionAsync(_channelId, _messageId, _emoji, cancellationToken);
            return createReactionResult.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(createReactionResult);
        }

        private Task<EventResponseResult> RespondToReactionAddAsync(IMessageReactionAdd? gatewayEvent, CancellationToken cancellationToken)
            => (gatewayEvent is not null)
                ? RespondToReactionAsync(gatewayEvent.MessageID, gatewayEvent.UserID, gatewayEvent.Emoji, cancellationToken)
                : Task.FromResult(EventResponseResult.FromSuccess());

        private Task<EventResponseResult> RespondToReactionRemoveAsync(IMessageReactionRemove? gatewayEvent, CancellationToken cancellationToken)
            => (gatewayEvent is not null)
                ? RespondToReactionAsync(gatewayEvent.MessageID, gatewayEvent.UserID, gatewayEvent.Emoji, cancellationToken)
                : Task.FromResult(EventResponseResult.FromSuccess());

        private async Task<EventResponseResult> RespondToReactionAsync(
            Snowflake messageId,
            Snowflake userId,
            IPartialEmoji emoji,
            CancellationToken cancellationToken)
        {
            if ((messageId.Value == _messageId.Value)
                && (userId.Value != _currentUserId.Value)
                && emoji.Name.HasValue
                && (emoji.Name.Value == _emoji))
            {
                var onClickedResult = await _onClickedAsync.Invoke(cancellationToken);
                if (!onClickedResult.IsSuccess)
                    return EventResponseResult.FromError(onClickedResult);
            }

            return EventResponseResult.FromSuccess();
        }

        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly Snowflake _channelId;
        private readonly Snowflake _currentUserId;
        private readonly string _emoji;
        private readonly Snowflake _messageId;
        private readonly OperationAction _onClickedAsync;
        private readonly IResponseDelegator<IMessageReactionAdd> _reactionAddDelegator;
        private readonly IResponseDelegator<IMessageReactionRemove> _reactionRemoveDelegator;
    }
}
