using System.Net.Http;
using System.Reactive.PlatformServices;
using System.Threading.Tasks;

using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

using Microsoft.AspNetCore.Components.WebAssembly.Browser;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Modix.Web.Client.Authentication;
using Modix.Web.Client.Diagnostics;
using Modix.Web.Protocol;

namespace Modix.Web.Client
{
    public static class EntryPoint
    {
        #pragma warning disable IDE1006 // MainAsync would not be valid
        public static async Task Main(string[] args)
        {
            ProtocolConfiguration.Apply();

            var hostBuilder = WebAssemblyHostBuilder.CreateDefault(args);

            hostBuilder.RootComponents.Add<ApplicationRoot>("#application-root");

            hostBuilder.Services
                .AddAuthentication()
                .AddAuthorizationCore()
                .AddSingleton<ISystemClock, DefaultSystemClock>()
                .AddSingleton<ILocalStorageManager, LocalStorageManager>()
                .AddSingleton(serviceProvider => GrpcChannel.ForAddress(
                    hostBuilder.HostEnvironment.BaseAddress,
                    new GrpcChannelOptions
                    {
                        Credentials = ChannelCredentials.Create(new SslCredentials(), CallCredentials.FromInterceptor((context, metadata) =>
                        {
                            var token = serviceProvider.GetRequiredService<IAuthenticationManager>().BearerToken;
                            if (!string.IsNullOrWhiteSpace(token))
                                metadata.Add("Authorization", $"Bearer {token}");

                            return Task.CompletedTask;
                        })),
                        HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler())
                    }))
                .AddDiagnostics()
                .AddTransient<ApplicationRootModel>()
                .AddTransient<HomePageModel>();

            await using var host = hostBuilder.Build();

            await host.RunAsync();
        }
        #pragma warning restore IDE1006
    }
}
