using System.Collections.Immutable;

using ProtoBuf;

namespace Modix.Web.Protocol.Guilds
{
    [ProtoContract]
    public class GetIdentifiersResponse
    {
        public GetIdentifiersResponse(ImmutableArray<GuildIdentifier> identifiers)
            => Identifiers = identifiers;

        // Private constructor and initters are needed for deserialization
        private GetIdentifiersResponse()
            => Identifiers = default;

        [ProtoMember(1)]
        public ImmutableArray<GuildIdentifier> Identifiers { get; init; }
    }
}
