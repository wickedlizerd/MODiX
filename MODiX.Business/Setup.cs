using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Remora.Discord.Gateway.Reaction;

using Modix.Business.Diagnostics;

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
                .AddDiagnostics(configuration)
                .AddGatewayReaction();
    }
}
