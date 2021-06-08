using ProtoBuf;

namespace Modix.Web.Protocol.Authentication
{
    [ProtoContract]
    public class StartLoginRequest
    {
        public StartLoginRequest(
            string redirectUri,
            string? state)
        {
            RedirectUri = redirectUri;
            State = state;
        }

        // Private constructor and initters are needed for deserialization
        private StartLoginRequest()
            => RedirectUri = null!;

        [ProtoMember(1)]
        public string RedirectUri { get; init; }

        [ProtoMember(2)]
        public string? State { get; init; }
    }
}
