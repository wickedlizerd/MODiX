using Microsoft.Extensions.DependencyInjection;

using ProtoBuf.Grpc.Client;

using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web.Client.Diagnostics
{
    public static class DiagnosticsSetup
    {
        public static IServiceCollection AddDiagnostics(this IServiceCollection services)
            => services
                .AddContractClient<IDiagnosticsContract>()
                .AddTransient<DiagnosticsViewModel>();
    }
}
