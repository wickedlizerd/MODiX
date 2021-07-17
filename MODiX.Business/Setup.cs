using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Modix.Business.Authorization;
using Modix.Business.Diagnostics;
using Modix.Business.Guilds;
using Modix.Business.Messaging;
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
                .AddMessaging(configuration)
                .AddAuthorization(configuration)
                .AddDiagnostics(configuration)
                .AddGuilds()
                .AddUsers(configuration);
    }
}
