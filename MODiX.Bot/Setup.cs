using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;

using Modix.Bot.Commands;
using Modix.Bot.Controls;
using Modix.Bot.Parsers;
using Modix.Business;

namespace Modix.Bot
{
    public static class Setup
    {
        public static IServiceCollection AddModixBot(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDiscordGateway(serviceProvider => serviceProvider.GetRequiredService<IOptions<DiscordConfiguration>>().Value.BotToken)
                .Configure<DiscordGatewayClientOptions>(options => options.Intents =
                    GatewayIntents.Guilds
                    | GatewayIntents.GuildMessages
                    | GatewayIntents.GuildMessageReactions)
                .AddHostedService<ModixBot>()
                .AddDiscordCommands()
                .AddModixCommands()
                .AddParsers()
                .AddControls();
    }
}
