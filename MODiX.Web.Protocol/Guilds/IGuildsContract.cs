using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Modix.Web.Protocol.Guilds
{
    [ServiceContract(Name = "Guilds")]
    public interface IGuildsContract
    {
        [OperationContract(Name = "GetIdentifiers")]
        Task<GetIdentifiersResponse> GetIdentifiersAsync(CancellationToken cancellationToken);
    }
}
