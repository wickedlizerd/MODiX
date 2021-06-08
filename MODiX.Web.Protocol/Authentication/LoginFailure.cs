using ProtoBuf;

namespace Modix.Web.Protocol.Authentication
{
    [ProtoContract]
    public class LoginFailure
        : CompleteLoginResponse
    {
        public LoginFailure(string message)
            => Message = message;

        // Private constructor and initters are needed for deserialization
        private LoginFailure()
            => Message = null!;

        [ProtoMember(1)]
        public string Message { get; init; }
    }
}
