using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public abstract class StartupActionBase
            : IHostedService
    {
        public StartupActionBase(ILogger logger)
            => Logger = logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            HostingLogMessages.StartupActionExecuting(Logger);
            await OnStartupAsync(cancellationToken);
            HostingLogMessages.StartupActionExecuted(Logger);
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        protected abstract Task OnStartupAsync(CancellationToken cancellationToken);

        protected readonly ILogger Logger;
    }
}
