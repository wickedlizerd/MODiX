using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public abstract class ReactiveBehaviorBase
        : IHostedService
    {
        public ReactiveBehaviorBase(ILogger<ReactiveBehaviorBase> logger)
            => Logger = logger;

        protected abstract IDisposable Start(IScheduler scheduler);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_stopToken is not null)
            {
                HostingLogMessages.BehaviorStopping(Logger);
                _stopToken?.Dispose();
                HostingLogMessages.BehaviorStopped(Logger);
            }

            HostingLogMessages.BehaviorStarting(Logger);
            _stopToken = Start(DefaultScheduler.Instance);
            HostingLogMessages.BehaviorStarted(Logger);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_stopToken is not null)
            {
                HostingLogMessages.BehaviorStopping(Logger);
                _stopToken?.Dispose();
                _stopToken = null;
                HostingLogMessages.BehaviorStopped(Logger);
            }

            return Task.CompletedTask;
        }

        protected readonly ILogger Logger;

        private IDisposable? _stopToken;
    }
}
