using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Modix.Business.Messaging
{
    public class UnknownEventResponder
        : IResponder<IUnknownEvent>
    {
        public UnknownEventResponder(ILogger<UnknownEventResponder> logger)
            => _logger = logger;

        public Task<Result> RespondAsync(
            IUnknownEvent       gatewayEvent,
            CancellationToken   cancellationToken)
        {
            MessagingLogMessages.UnknownEventReceived(_logger, gatewayEvent);

            return Task.FromResult(Result.FromSuccess());
        }

        private readonly ILogger _logger;
    }
}
