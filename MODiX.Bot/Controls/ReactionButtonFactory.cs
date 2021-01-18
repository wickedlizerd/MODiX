using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.Core;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Delegation;
using Remora.Results;

namespace Modix.Bot.Controls
{
    public interface IReactionButtonFactory
    {
        Task<ControlCreationResult<ReactionButton>> CreateAsync(
            Snowflake channelId,
            Snowflake messageId,
            string emoji,
            OperationAction onClickedAsync,
            CancellationToken cancellationToken = default);
    }

    public class ReactionButtonFactory
        : IReactionButtonFactory
    {
        public ReactionButtonFactory(
            IDiscordRestChannelAPI channelApi,
            IDiscordRestUserAPI userApi,
            IResponseDelegator<IMessageReactionAdd> reactionAddDelegator,
            IResponseDelegator<IMessageReactionRemove> reactionRemoveDelegator)
        {
            _channelApi = channelApi;
            _userApi = userApi;
            _reactionAddDelegator = reactionAddDelegator;
            _reactionRemoveDelegator = reactionRemoveDelegator;
        }

        public Task<ControlCreationResult<ReactionButton>> CreateAsync(
                Snowflake channelId,
                Snowflake messageId,
                string emoji,
                OperationAction onClickedAsync,
                CancellationToken cancellationToken = default)
            => ReactionButton.CreateAsync(
                _channelApi,
                _userApi,
                _reactionAddDelegator,
                _reactionRemoveDelegator,
                channelId,
                messageId,
                emoji,
                onClickedAsync,
                cancellationToken);

        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly IDiscordRestUserAPI _userApi;
        private readonly IResponseDelegator<IMessageReactionAdd> _reactionAddDelegator;
        private readonly IResponseDelegator<IMessageReactionRemove> _reactionRemoveDelegator;
    }
}
