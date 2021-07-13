using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Modix.Web.Protocol.Guilds;

namespace Modix.Web.Server.Guilds
{
    public static class Setup
    {
        public static IServiceCollection AddGuilds(this IServiceCollection services)
            => services
                .AddScopedWithAlias<GuildsContract, IGuildsContract>();

        public static IEndpointRouteBuilder MapGuilds(this IEndpointRouteBuilder endpoints)
            => endpoints
                .Map(endpoints => endpoints.MapGrpcService<GuildsContract>());
    }
}
