using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using Remora.Discord.Gateway;

namespace Modix.Bot
{
    public class ModixBot
        : BackgroundService
    {
        public ModixBot(DiscordGatewayClient client)
        {
            _client = client;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => _client.RunAsync(stoppingToken);

        private readonly DiscordGatewayClient _client;
    }
}
