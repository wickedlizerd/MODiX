using System.Threading;
using System.Threading.Tasks;

namespace Remora.Results
{
    public delegate Task<OperationResult> OperationAction(CancellationToken cancellationToken);
}
