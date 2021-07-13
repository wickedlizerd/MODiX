using ProtoBuf;

namespace Modix.Web.Protocol.Guilds
{
    [ProtoContract]
    public class GuildIdentifier
    {
        public GuildIdentifier(
            ulong   id,
            string  name,
            string? iconHash)
        {
            Id          = id;
            Name        = name;
            IconHash    = iconHash;
        }

        // Private constructor and initters are needed for deserialization
        private GuildIdentifier()
            => Name = null!;

        [ProtoMember(1)]
        public ulong Id { get; init; }

        [ProtoMember(2)]
        public string Name { get; init; }

        [ProtoMember(3)]
        public string? IconHash { get; init; }
    }
}
