using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Modix.Business.Guilds.Tracking;
using Modix.Web.Protocol.Guilds;

namespace Modix.Web.Server.Guilds
{
    public class GuildsContract
        : IGuildsContract
    {
        public GuildsContract(IGuildTrackingCache guildTrackingCache)
            => _guildTrackingCache = guildTrackingCache;

        public Task<GetIdentifiersResponse> GetIdentifiersAsync(CancellationToken cancellationToken)
            => Task.FromResult(new GetIdentifiersResponse(
                identifiers: _guildTrackingCache.EnumerateEntries()
                    .Select(entry => new GuildIdentifier(
                        id:         entry.Id.Value,
                        name:       entry.Name,
                        iconHash:   entry.Icon?.Value))
                    .ToImmutableArray()));

        private readonly IGuildTrackingCache _guildTrackingCache;
    }
}
