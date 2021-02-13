using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public interface IMessageDialogFactory
    {
        Task<MessageDialog> CreateAsync(
            Snowflake? guildId,
            Snowflake channelId,
            IReadOnlyList<string> buttonEmojiNames,
            Optional<string> content = default,
            Optional<IEmbed> embed = default);
    }

    public class MessageDialogFactory
        : IMessageDialogFactory
    {
        public MessageDialogFactory(
            IDiscordRestChannelAPI channelApi,
            IReactionButtonFactory buttonFactory,
            IObservable<IGuildDelete> guildDeleted,
            IObservable<IChannelDelete> channelDeleted,
            IObservable<IMessageDelete> messageDeleted)
        {
            _channelApi = channelApi;
            _buttonFactory = buttonFactory;
            _guildDeleted = guildDeleted;
            _channelDeleted = channelDeleted;
            _messageDeleted = messageDeleted;
        }

        public Task<MessageDialog> CreateAsync(
                Snowflake? guildId,
                Snowflake channelId,
                IReadOnlyList<string> buttonEmojiNames,
                Optional<string> content = default,
                Optional<IEmbed> embed = default)
            => MessageDialog.CreateAsync(
                channelApi:         _channelApi,
                buttonFactory:      _buttonFactory,
                guildDeleted:       _guildDeleted,
                channelDeleted:     _channelDeleted,
                messageDeleted:     _messageDeleted,
                guildId:            guildId,
                channelId:          channelId,
                buttonEmojiNames:   buttonEmojiNames,
                content:            content,
                embed:              embed);

        private readonly IDiscordRestChannelAPI _channelApi;
        private readonly IReactionButtonFactory _buttonFactory;
        private readonly IObservable<IGuildDelete> _guildDeleted;
        private readonly IObservable<IChannelDelete> _channelDeleted;
        private readonly IObservable<IMessageDelete> _messageDeleted;
    }
}
