using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Modix.Host.Logging;

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
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var keyPerFilePath = context.Configuration.GetSection("MODiX:Host:SecretsFilesPath").Value;
                    if (!string.IsNullOrWhiteSpace(keyPerFilePath))
                        builder.AddKeyPerFile(keyPerFilePath);
                })
                .ConfigureWebHostDefaults(webHost => webHost
                    .UseStartup<Startup>())
                .ConfigureLogging()
                .Build();

            host.Run();
        }
    }
}
