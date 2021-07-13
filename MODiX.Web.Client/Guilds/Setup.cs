using Microsoft.Extensions.DependencyInjection;

using ProtoBuf.Grpc.Client;

using Modix.Web.Protocol.Guilds;

namespace Modix.Web.Client.Guilds
{
    public static class Setup
    {
        public static IServiceCollection AddGuilds(this IServiceCollection services)
            => services.AddContractClient<IGuildsContract>();
    }
}
