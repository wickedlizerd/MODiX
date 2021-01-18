using System.Net.Http;
using System.Threading.Tasks;

using Grpc.Net.Client;
using Grpc.Net.Client.Web;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using ProtoBuf.Grpc.Client;

using Modix.Web.Client.Diagnostics;
using Modix.Web.Protocol.Diagnostics;

namespace Modix.Web
{
    public static class EntryPoint
    {
        #pragma warning disable IDE1006 // MainAsync would not be valid
        public static async Task Main(string[] args)
        {
            var hostBuilder = WebAssemblyHostBuilder.CreateDefault(args);

            hostBuilder.RootComponents.Add<ApplicationView>("#application-root");

            hostBuilder.Services
                .AddSingleton(services => GrpcChannel.ForAddress(
                    hostBuilder.HostEnvironment.BaseAddress,
                    new GrpcChannelOptions
                    {
                        HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler())
                    }))
                .AddSingleton(services => services.GetRequiredService<GrpcChannel>().CreateGrpcService<IDiagnosticsContract>())
                .AddTransient<ApplicationViewModel>()
                .AddTransient<DiagnosticsViewModel>();

            await using var host = hostBuilder.Build();

            await host.RunAsync();
        }
        #pragma warning restore IDE1006
    }
}
