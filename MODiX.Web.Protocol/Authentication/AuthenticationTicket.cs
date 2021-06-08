using System;

using ProtoBuf;

namespace Modix.Web.Protocol.Authentication
{
    [ProtoContract]
    public class AuthenticationTicket
    {
        public AuthenticationTicket(
            ulong userId,
            DateTimeOffset created,
            DateTimeOffset expires)
        {
            UserId = userId;
            Created = created;
            Expires = expires;
        }

        // Private constructor and initters are needed for deserialization
        private AuthenticationTicket() { }

        [ProtoMember(1)]
        public ulong UserId { get; init; }

        [ProtoMember(2)]
        public DateTimeOffset Created { get; init; }

        [ProtoMember(3)]
        public DateTimeOffset Expires { get; init; }
    }
}
