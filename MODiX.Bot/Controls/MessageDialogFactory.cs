using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.Core;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;

using System.Collections.Generic;

namespace Modix.Bot.Controls
{
    public interface IMessageDialogFactory
    {
        Task<ControlCreationResult<MessageDialog>> CreateAsync(
            IEnumerable<ReactionButtonOptions> buttons,
            Snowflake channelId,
            Optional<string> content = default,
            Optional<IEmbed> embed = default,
            CancellationToken cancellationToken = default);
    }

    public class MessageDialogFactory
        : IMessageDialogFactory
    {
        public MessageDialogFactory(
            IReactionButtonFactory buttonFactory,
            IDiscordRestChannelAPI channelApi)
        {
            _buttonFactory = buttonFactory;
            _channelApi = channelApi;
        }

        public Task<ControlCreationResult<MessageDialog>> CreateAsync(
                IEnumerable<ReactionButtonOptions> buttons,
                Snowflake channelId,
                Optional<string> content = default,
                Optional<IEmbed> embed = default,
                CancellationToken cancellationToken = default)
            => MessageDialog.CreateAsync(
                _buttonFactory,
                _channelApi,
                buttons,
                channelId,
                content,
                embed,
                cancellationToken);

        private readonly IReactionButtonFactory _buttonFactory;
        private readonly IDiscordRestChannelAPI _channelApi;
    }
}
