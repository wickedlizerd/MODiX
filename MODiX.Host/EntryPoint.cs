using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Modix.Host
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
        {
            using var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
                .ConfigureWebHostDefaults(webHost => webHost
                    .UseStartup<Startup>())
                .Build();

            host.Run();
        }
    }
}
