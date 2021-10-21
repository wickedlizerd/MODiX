using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.PlatformServices;
using System.Reflection;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SeqLoggerProvider.Serialization;

using Modix.Bot;
using Modix.Business;
using Modix.Data;
using Modix.Data.Migrations;
using Modix.Web.Server;

namespace Modix.Host
{
    public class EntryPoint
    {
        public static void Main()
        {
            using var host = new HostBuilder()
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
                    options.ValidateScopes  = context.HostingEnvironment.IsDevelopment();
                })
                .ConfigureHostConfiguration(builder => builder
                    .AddEnvironmentVariables(prefix: "DOTNET_"))
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);

                    if (context.HostingEnvironment.IsDevelopment())
                        builder.AddUserSecrets<EntryPoint>(optional: true);

                    var secretsFilesPath = context.Configuration["SECRETS_FILES_PATH"];
                    if (!string.IsNullOrWhiteSpace(secretsFilesPath))
                        builder.AddKeyPerFile(secretsFilesPath);
                })
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
                                options.ReferenceHandler = ReferenceHandler.Preserve;
                                options.Converters.Add(new NullableContextAttributeWriteOnlyJsonConverter());
                                options.Converters.Add(new SnowflakeWriteOnlyJsonConverter());
                            })))
                .ConfigureServices((context, services) => services
                    .AddResponseCompression(options => options
                        .MimeTypes = ResponseCompressionDefaults.MimeTypes
                            .Append("application/octet-stream"))
                    .AddSingleton<ISystemClock, DefaultSystemClock>()
                    .AddModixBot()
                    .AddModixBusiness(context.Configuration)
                    .AddModixData(context.Configuration)
                    .AddModixDataMigrations()
                    .AddModixWebServer(context.Configuration))
                .ConfigureWebHost(builder => builder
                    .UseKestrel((context, options) => options
                        .Configure(context.Configuration.GetSection("Kestrel")))
                    .ConfigureAppConfiguration((context, _) =>
                    {
                        if (context.HostingEnvironment.IsDevelopment())
                            StaticWebAssetsLoader.UseStaticWebAssets(context.HostingEnvironment, context.Configuration);
                    })
                    .Configure((context, builder) =>
                    {
                        builder
                            .UseResponseCompression();

                        if (context.HostingEnvironment.IsDevelopment())
                            builder
                                .UseDeveloperExceptionPage()
                                .UseWebAssemblyDebugging();

                        builder
                            .UseBlazorFrameworkFiles()
                            .UseStaticFiles()
                            .UseModixWebServer()
                            .UseFallbackFile("index.html");
                    }))
                .Build();

            host.Run();
        }
    }
}
