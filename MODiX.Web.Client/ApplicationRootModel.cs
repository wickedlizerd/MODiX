using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Modix.Web.Client.Authentication;
using Modix.Web.Protocol.Guilds;

namespace Modix.Web.Client
{
    public class ApplicationRootModel
    {
        public ApplicationRootModel(
            IAuthenticationManager  authenticationManager,
            IGuildsContract         guildsContract)
        {
            AvailableGuilds = Observable.FromAsync(cancellationToken => guildsContract.GetIdentifiersAsync(cancellationToken))
                .Select(response => response.Identifiers)
                .ShareReplay(1);

            var selectedGuild = Observable.CombineLatest(
                    authenticationManager.ActiveGuildId,
                    AvailableGuilds,
                    (activeGuildId, availableGuilds) =>
                    {
                        var activeGuild = availableGuilds.FirstOrDefault(guild => guild.Id == activeGuildId);
                        if (activeGuild is null)
                        {
                            activeGuild = availableGuilds.First();
                            authenticationManager.ActivateGuild(activeGuild.Id);
                        }

                        return activeGuild;
                    })
                .StartWith(default(GuildIdentifier?))
                .ShareReplay(1);

            IsGuildSelected = selectedGuild
                .Select(guild => guild is not null);

            SelectedGuildName = selectedGuild
                .WhereNotNull()
                .Select(selectedGuild => selectedGuild.Name);

            SelectedGuildIconUri = selectedGuild
                .WhereNotNull()
                .Select(guild => (guild.IconHash is null)   ? null
                    : guild.IconHash.StartsWith("a_")       ? $"https://cdn.discordapp.com/icons/{guild.Id}/{guild.IconHash}.gif?size=64"
                                                            : $"https://cdn.discordapp.com/icons/{guild.Id}/{guild.IconHash}.png?size=64");
        }

        public IObservable<ImmutableArray<GuildIdentifier>> AvailableGuilds { get; }

        public IObservable<bool> IsGuildSelected { get; }

        public IObservable<string> SelectedGuildName { get; }

        public IObservable<string?> SelectedGuildIconUri { get; }
    }
}
