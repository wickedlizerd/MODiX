using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public abstract class ReactiveBehaviorBase
        : IHostedService
    {
        protected abstract IDisposable Start(IScheduler scheduler);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _stopToken?.Dispose();
            _stopToken = Start(DefaultScheduler.Instance);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _stopToken?.Dispose();
            _stopToken = null;

            return Task.CompletedTask;
        }

        private IDisposable? _stopToken;
    }
}
