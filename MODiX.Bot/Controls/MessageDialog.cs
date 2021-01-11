using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;
using Remora.Results;

namespace Modix.Bot.Controls
{
    public class MessageDialog
        : ControlBase
    {
        public static async Task<ControlCreationResult<MessageDialog>> CreateAsync(
            IReactionButtonFactory buttonFactory,
            IDiscordRestChannelAPI channelApi,
            IEnumerable<ReactionButtonOptions> buttons,
            Snowflake channelId,
            Optional<string> content = default,
            Optional<IEmbed> embed = default,
            CancellationToken cancellationToken = default)
        {
            var createMessageResult = await channelApi.CreateMessageAsync(
                channelID:  channelId,
                content:    content,
                embed:      embed);
            if (!createMessageResult.IsSuccess)
                return ControlCreationResult<MessageDialog>.FromError(createMessageResult);

            var buttonCreationResults = await Task.WhenAll(buttons
                .Select(button => buttonFactory.CreateAsync(
                    channelId:          channelId,
                    messageId:          createMessageResult.Entity.ID,
                    emoji:              button.Emoji,
                    onClickedAsync:     button.OnClickedAsync,
                    cancellationToken:  cancellationToken)));
            var failedButtonCreationResults = buttonCreationResults
                .Where(result => !result.IsSuccess)
                .ToArray();
            if (failedButtonCreationResults.Length != 0)
            {
                var buttonDestructionResults = await Task.WhenAll(failedButtonCreationResults
                    .Select(result => result.Control.DestroyAsync()));

                return ControlCreationResult<MessageDialog>.FromError(AggregateResult.FromResults(
                    Enumerable.Concat<IResult>(
                        failedButtonCreationResults,
                        buttonDestructionResults.Where(result => !result.IsSuccess))));
            }

            var dialog = new MessageDialog(
                buttons:    buttonCreationResults
                    .Select(result => result.Control)
                    .ToArray(),
                channelApi: channelApi,
                channelId:  channelId,
                messageId:  createMessageResult.Entity.ID);

            return await OnCreatingAsync(dialog, cancellationToken);
        }

        private MessageDialog(
            IReadOnlyList<ReactionButton> buttons,
            IDiscordRestChannelAPI channelApi,
            Snowflake channelId,
            Snowflake messageId)
        {
            _buttons = buttons;
            _channelApi = channelApi;
            _channelId = channelId;
            _messageId = messageId;
        }


        public override async Task<OperationResult> DestroyAsync()
        {
            var results = await Task.WhenAll(_buttons
                .Select(button => button.DestroyAsync())
                .Append(base.DestroyAsync()));

            var failedResults = results.Where(result => !result.IsSuccess);
            return failedResults.Any()
                ? OperationResult.FromError(AggregateResult.FromResults(results))
                : OperationResult.FromSuccess();
        }

        public async Task<OperationResult> UpdateAsync(
            Optional<string?> content = default,
            Optional<IEmbed?> embed = default,
            CancellationToken cancellationToken = default)
        {
            var editMessageResult = await _channelApi.EditMessageAsync(
                channelID:  _channelId,
                messageID:  _messageId,
                content:    content,
                embed:      embed,
                ct:         cancellationToken);
            return editMessageResult.IsSuccess
                ? OperationResult.FromSuccess()
                : OperationResult.FromError(editMessageResult);
        }

        private readonly IReadOnlyList<ReactionButton> _buttons;
        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly Snowflake _channelId;
        private readonly Snowflake _messageId;
    }
}
