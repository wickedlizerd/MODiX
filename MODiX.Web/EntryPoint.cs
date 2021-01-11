using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

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
                .AddTransient<ApplicationViewModel>();

            await using var host = hostBuilder.Build();

            await host.RunAsync();
        }
        #pragma warning restore IDE1006
    }
}
