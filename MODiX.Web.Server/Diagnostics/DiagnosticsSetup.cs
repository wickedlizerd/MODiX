using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web.Server.Diagnostics
{
    public static class DiagnosticsSetup
    {
        public static IServiceCollection AddDiagnostics(this IServiceCollection services)
            => services
                .AddScopedWithAlias<DiagnosticsContract, IDiagnosticsContract>();

        public static IEndpointRouteBuilder MapDiagnostics(this IEndpointRouteBuilder endpoints)
            => endpoints
                .Map(endpoints => endpoints.MapGrpcService<DiagnosticsContract>());
    }
}
