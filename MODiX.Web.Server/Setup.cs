using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProtoBuf.Grpc.Server;

using Modix.Web.Protocol.Diagnostics;
using Modix.Web.Server.Diagnostics;

namespace Modix.Web.Server
{
    public static class Setup
    {
        public static IServiceCollection AddModixWebServer(this IServiceCollection services)
            => services
                .Add(services => services.AddCodeFirstGrpc())
                .AddScoped<IDiagnosticsContract, DiagnosticsContract>();

        public static IApplicationBuilder UseModixWebServer(this IApplicationBuilder application)
            => application
                .UseRouting()
                .UseGrpcWeb(new GrpcWebOptions()
                {
                    DefaultEnabled = true
                })
                .UseEndpoints(endpoints => endpoints
                    .Map(endpoints => endpoints.MapGrpcService<IDiagnosticsContract>()));
    }
}
