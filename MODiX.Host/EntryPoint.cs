using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                    var secretsFilesPath = context.Configuration["SECRETS_FILES_PATH"];
                    if (!string.IsNullOrWhiteSpace(secretsFilesPath))
                        builder.AddKeyPerFile(secretsFilesPath);
                })
                .ConfigureWebHostDefaults(webHost => webHost
                    .UseStartup<Startup>())
                .ConfigureLogging((context, builder) => builder
                    .AddConfiguration(context.Configuration.GetSection("MODiX:Host:Logging"))
                    .AddConsole()
                    .AddDebug()
                    .AddSeq(
                        configure:                  builder => builder
                            .Configure(options =>
                            {
                                var assembly = Assembly.GetExecutingAssembly();
                                var assemblyName = assembly.GetName();
                                var globalFields = new Dictionary<string, string>();

                                if (assemblyName.Name is not null)
                                    globalFields.Add("Application", assemblyName.Name);

                                var applicationVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
                                if (applicationVersion is not null)
                                    globalFields.Add("ApplicationVersion", applicationVersion);

                                if (options.GlobalFields is not null)
                                    foreach (var field in options.GlobalFields)
                                        globalFields.Add(field.Key, field.Value);

                                options.GlobalFields = globalFields;
                            })
                            .ValidateOnStartup(),
                        configureJsonSerializer:    builder => builder
                            .Configure(options =>
                            {
                                options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                                options.Converters.Add(new NullableContextAttributeWriteOnlyJsonConverter());
                            })
                        ))
                .Build();

            host.Run();
        }
    }
}
