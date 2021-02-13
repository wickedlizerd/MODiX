using System;
using System.Reactive.Linq;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Core;

namespace Modix.Bot.Controls
{
    public abstract class ControlBase
        : AsyncDisposableBase
    {
        protected ControlBase(
                Snowflake? guildId,
                IObservable<IGuildDelete> guildDeleted,
                IObservable<ControlException> hostDeleted)
        {
            _guildId = guildId;
            _hostDeleted = (guildId.HasValue
                    ? Observable.Merge(
                        hostDeleted,
                        guildDeleted
                            .Where(@event => @event.GuildID == guildId.Value)
                            .Select(@event => new ControlException($"The guild hosting this control {((@event.IsUnavailable.HasValue && @event.IsUnavailable.Value) ? "is unavailable" : "was deleted")}")))
                    : hostDeleted)
                .DoOnError(_ => _isHostDeleted = true);
        }

        protected Snowflake? GuildId
            => _guildId;

        protected IObservable<ControlException> HostDeleted
            => _hostDeleted;

        protected bool IsHostDeleted
            => _isHostDeleted;

        private readonly Snowflake? _guildId;
        private readonly IObservable<ControlException> _hostDeleted;

        private bool _isHostDeleted;
    }
}
