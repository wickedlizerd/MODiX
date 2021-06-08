using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProtoBuf.Grpc.Server;

using Modix.Web.Server.Authentication;
using Modix.Web.Server.Diagnostics;
using Modix.Web.Protocol;

namespace Modix.Web.Server
{
    public static class Setup
    {
        public static IServiceCollection AddModixWebServer(this IServiceCollection services, IConfiguration configuration)
        {
            ProtocolConfiguration.Apply();

            return services
                .Add(services => services.AddCodeFirstGrpc())
                .AddAuthentication(configuration)
                .AddAuthorization()
                .AddDiagnostics();
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
                    .MapDiagnostics());
    }
}
