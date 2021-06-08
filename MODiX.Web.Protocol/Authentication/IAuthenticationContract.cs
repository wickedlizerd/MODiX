using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Modix.Web.Protocol.Authentication
{
    [ServiceContract(Name = "Authentication")]
    public interface IAuthenticationContract
    {
        [OperationContract(Name = "CompleteLogin")]
        Task<CompleteLoginResponse> CompleteLoginAsync(CompleteLoginRequest request, CancellationToken cancellationToken);

        [OperationContract(Name = "StartLogin")]
        Task<StartLoginResponse> StartLoginAsync(StartLoginRequest request, CancellationToken cancellationToken);
    }
}
