using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Modix.Host
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
        {
            using var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHost => webHost
                    .UseStartup<Startup>())
                .Build();

            host.Run();
        }
    }
}
