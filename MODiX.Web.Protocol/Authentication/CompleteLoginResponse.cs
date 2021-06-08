using ProtoBuf;

namespace Modix.Web.Protocol.Authentication
{
    [ProtoContract]
    [ProtoInclude(1, typeof(LoginSuccess))]
    [ProtoInclude(2, typeof(LoginFailure))]
    public abstract class CompleteLoginResponse { }
}
