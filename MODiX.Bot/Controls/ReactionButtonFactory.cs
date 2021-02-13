using System;
using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public interface IReactionButtonFactory
    {
        Task<ReactionButton> CreateAsync(
            Snowflake? guildId,
            Snowflake channelId,
            Snowflake messageId,
            string emojiName);
    }

    public class ReactionButtonFactory
        : IReactionButtonFactory
    {
        public ReactionButtonFactory(
            IDiscordRestChannelAPI channelApi,
            IObservable<IChannelDelete> channelDeleted,
            IObservable<IGuildDelete> guildDeleted,
            IObservable<IMessageDelete> messageDeleted,
            IObservable<IMessageReactionAdd> messageReactionAdded,
            IObservable<IMessageReactionRemove> messageReactionRemoved,
            IDiscordRestUserAPI userApi)
        {
            _channelApi = channelApi;
            _channelDeleted = channelDeleted;
            _guildDeleted = guildDeleted;
            _messageDeleted = messageDeleted;
            _messageReactionAdded = messageReactionAdded;
            _messageReactionRemoved = messageReactionRemoved;
            _userApi = userApi;
        }

        public Task<ReactionButton> CreateAsync(
                Snowflake? guildId,
                Snowflake channelId,
                Snowflake messageId,
                string emojiName)
            => ReactionButton.CreateAsync(
                channelApi:             _channelApi,
                userApi:                _userApi,
                guildDeleted:           _guildDeleted,
                channelDeleted:         _channelDeleted,
                messageDeleted:         _messageDeleted,
                messageReactionAdded:   _messageReactionAdded,
                messageReactionRemoved: _messageReactionRemoved,
                guildId:                guildId,
                channelId:              channelId,
                messageId:              messageId,
                emojiName:              emojiName);

        private readonly IDiscordRestChannelAPI                 _channelApi;
        private readonly IObservable<IChannelDelete>            _channelDeleted;
        private readonly IObservable<IGuildDelete>              _guildDeleted;
        private readonly IObservable<IMessageDelete>            _messageDeleted;
        private readonly IObservable<IMessageReactionAdd>       _messageReactionAdded;
        private readonly IObservable<IMessageReactionRemove>    _messageReactionRemoved;
        private readonly IDiscordRestUserAPI                    _userApi;
    }
}
