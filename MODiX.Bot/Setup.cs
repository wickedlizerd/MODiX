using Microsoft.Extensions.DependencyInjection;
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
        public static IServiceCollection AddModixBot(this IServiceCollection services)
            => services
                .AddDiscordGateway(serviceProvider => serviceProvider.GetRequiredService<IOptions<DiscordConfiguration>>().Value.BotToken)
                .Configure<DiscordGatewayClientOptions>(options => options.Intents =
                    GatewayIntents.Guilds
                    | GatewayIntents.GuildMembers
                    | GatewayIntents.GuildMessages
                    | GatewayIntents.GuildMessageReactions
                    | GatewayIntents.GuildPresences)
                .AddHostedService<ModixBot>()
                .AddDiscordCommands()
                .AddModixCommands()
                .AddParsers()
                .AddControls();
    }
}
