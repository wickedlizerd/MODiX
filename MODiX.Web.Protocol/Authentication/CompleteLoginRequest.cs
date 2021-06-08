using ProtoBuf;

namespace Modix.Web.Protocol.Authentication
{
    [ProtoContract]
    public class CompleteLoginRequest
    {
        public CompleteLoginRequest(
            string code,
            string redirectUri)
        {
            Code = code;
            RedirectUrl = redirectUri;
        }

        // Private constructor and initters are needed for deserialization
        private CompleteLoginRequest()
        {
            Code = null!;
            RedirectUrl = null!;
        }

        [ProtoMember(1)]
        public string Code { get; init; }

        [ProtoMember(2)]
        public string RedirectUrl { get; init; }
    }
}
