using ProtoBuf;

namespace Modix.Web.Protocol.Authentication
{
    [ProtoContract]
    public class StartLoginResponse
    {
        public StartLoginResponse(string authorizeUrl)
            => AuthorizeUri = authorizeUrl;

        // Private constructor and initters are needed for deserialization
        private StartLoginResponse()
            => AuthorizeUri = null!;

        [ProtoMember(1)]
        public string AuthorizeUri { get; init; }
    }
}
