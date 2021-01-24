using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public sealed class MessageDialog
        : MessageControlBase
    {
        public static async Task<MessageDialog> CreateAsync(
            IDiscordRestChannelAPI channelApi,
            IReactionButtonFactory buttonFactory,
            IObservable<IGuildDelete?> guildDeleted,
            IObservable<IChannelDelete?> channelDeleted,
            IObservable<IMessageDelete?> messageDeleted,
            Snowflake? guildId,
            Snowflake channelId,
            IReadOnlyList<string> buttonEmojiNames,
            Optional<string> content,
            Optional<IEmbed> embed)
        {
            var createMessageResult = await channelApi.CreateMessageAsync(
                channelID:  channelId,
                content:    content,
                embed:      embed);
            if (!createMessageResult.IsSuccess)
                throw new ControlException($"Uncable to create dialog: {createMessageResult.ErrorReason}", createMessageResult.Exception);

            var buttons = (await Task.WhenAll(buttonEmojiNames
                    .Select(buttonEmojiName => buttonFactory.CreateAsync(
                        guildId:    guildId,
                        channelId:  channelId,
                        messageId:  createMessageResult.Entity.ID,
                        emojiName:  buttonEmojiName))))
                .ToImmutableArray();

            return new MessageDialog(
                guildId:        guildId,
                channelId:      channelId,
                messageId:      createMessageResult.Entity.ID,
                guildDeleted:   guildDeleted,
                channelDeleted: channelDeleted,
                messageDeleted: messageDeleted,
                buttons:        buttons,
                channelApi:     channelApi);
        }

        private MessageDialog(
                Snowflake? guildId,
                Snowflake channelId,
                Snowflake messageId,
                IObservable<IGuildDelete?> guildDeleted,
                IObservable<IChannelDelete?> channelDeleted,
                IObservable<IMessageDelete?> messageDeleted,
                ImmutableArray<ReactionButton> buttons,
                IDiscordRestChannelAPI channelApi)
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

            _buttons = buttons;
            _buttonClicked = Observable.Merge(
                    HostDeleted.Throw<ReactionButtonClickedEvent>(),
                    Observable.Merge(buttons
                        .Select(button => button.Clicked)))
                .Share();
        }

        public IObservable<ReactionButtonClickedEvent> ButtonClicked
            => _buttonClicked;

        public async Task UpdateAsync(
            Optional<string?> content = default,
            Optional<IEmbed?> embed = default)
        {
            var editMessageResult = await _channelApi.EditMessageAsync(
                channelID:  ChannelId,
                messageID:  MessageId,
                content:    content,
                embed:      embed);
            if (!editMessageResult.IsSuccess)
                throw new ControlException($"Unable to update the dialog: {editMessageResult.ErrorReason}", editMessageResult.Exception);
        }

        protected override async ValueTask OnDisposingAsync(DisposalType type)
        {
            if (!IsHostDeleted)
            {
                await Task.WhenAll(_buttons
                    .Select(button => button.DisposeAsync().AsTask()));

                await _channelApi.DeleteMessageAsync(ChannelId, MessageId);
            }

            await base.OnDisposingAsync(type);
        }

        private readonly ImmutableArray<ReactionButton> _buttons;
        private readonly IObservable<ReactionButtonClickedEvent> _buttonClicked;
        private readonly IDiscordRestChannelAPI _channelApi;
    }
}
