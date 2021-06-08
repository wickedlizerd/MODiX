using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using ProtoBuf.Grpc.Server;

using Modix.Web.Server.Authentication;
using Modix.Web.Server.Diagnostics;
using Modix.Web.Protocol;
using Modix.Web.Protocol.Authentication;
using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web.Server
{
    public static class Setup
    {
        public static IServiceCollection AddModixWebServer(this IServiceCollection services, IConfiguration configuration)
        {
            ProtocolConfiguration.Apply();

            return services
                .Add(services => services.AddCodeFirstGrpc())
                .AddScoped<IAuthenticationContract, AuthenticationContract>()
                .AddScoped<IDiagnosticsContract, DiagnosticsContract>()
                .Add(services => services.AddOptions<AuthenticationConfiguration>()
                    .Bind(configuration.GetSection("Authentication"))
                    .ValidateDataAnnotations()
                    .ValidateOnStartup());
        }

        public static IApplicationBuilder UseModixWebServer(this IApplicationBuilder application)
            => application
                .UseRouting()
                .UseGrpcWeb(new GrpcWebOptions()
                {
                    DefaultEnabled = true
                })
                .UseEndpoints(endpoints => endpoints
                    .Map(endpoints => endpoints.MapGrpcService<IAuthenticationContract>())
                    .Map(endpoints => endpoints.MapGrpcService<IDiagnosticsContract>()));
    }
}
