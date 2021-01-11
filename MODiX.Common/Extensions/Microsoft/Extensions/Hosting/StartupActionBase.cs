using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public abstract class StartupActionBase
            : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
            => OnStartupAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        protected abstract Task OnStartupAsync(CancellationToken cancellationToken);
    }
}
