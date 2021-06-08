using ProtoBuf;

namespace Modix.Web.Protocol.Authentication
{
    [ProtoContract]
    public class LoginSuccess
        : CompleteLoginResponse
    {
        public LoginSuccess(
            AuthenticationTicket ticket,
            string bearerToken)
        {
            Ticket      = ticket;
            BearerToken = bearerToken;
        }

        // Private constructor and initters are needed for deserialization
        private LoginSuccess()
        {
            Ticket      = null!;
            BearerToken = null!;
        }

        [ProtoMember(1)]
        public AuthenticationTicket Ticket { get; init; }

        [ProtoMember(2)]
        public string BearerToken { get; init; }
    }
}
