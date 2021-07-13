using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Remora.Discord.Gateway.Reaction;

using Modix.Business.Authorization;
using Modix.Business.Diagnostics;
using Modix.Business.Guilds;
using Modix.Business.Users;

namespace Modix.Business
{
    public static class Setup
    {
        public static IServiceCollection AddModixBusiness(this IServiceCollection services, IConfiguration configuration)
            => services
                .Add(services => services.AddOptions<DiscordConfiguration>()
                    .Bind(configuration.GetSection("Discord"))
                    .ValidateDataAnnotations()
                    .ValidateOnStartup())
                .AddGatewayReaction()
                .AddAuthorization(configuration)
                .AddDiagnostics(configuration)
                .AddGuilds()
                .AddUsers(configuration);
    }
}
