using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProtoBuf.Grpc.Server;

using Modix.Web.Protocol;
using Modix.Web.Server.Authentication;
using Modix.Web.Server.Authorization;
using Modix.Web.Server.Diagnostics;
using Modix.Web.Server.Guilds;

namespace Modix.Web.Server
{
    public static class Setup
    {
        public static IServiceCollection AddModixWebServer(this IServiceCollection services, IConfiguration configuration)
        {
            ProtocolConfiguration.Apply();

            return services
                .AddHttpContextAccessor()
                .Add(services => services.AddCodeFirstGrpc())
                .AddAuthentication(configuration)
                .AddModixAuthorization()
                .AddDiagnostics()
                .AddGuilds();
        }

        public static IApplicationBuilder UseModixWebServer(this IApplicationBuilder application)
            => application
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseGrpcWeb(new GrpcWebOptions()
                {
                    DefaultEnabled = true
                })
                .UseEndpoints(endpoints => endpoints
                    .MapAuthentication()
                    .MapDiagnostics()
                    .MapGuilds());
    }
}
