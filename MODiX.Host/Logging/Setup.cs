using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Serilog;

namespace Modix.Host.Logging
{
    public static class Setup
    {
        public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
            => hostBuilder
                .UseSerilog((context, serviceProvider, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .MinimumLevel.Verbose()
                        .Enrich.FromLogContext()
                        .WriteTo.Logger(consoleLoggerConfiguration =>
                        {
                            consoleLoggerConfiguration
                                .ReadFrom.Configuration(context.Configuration.GetSection("MODiX:Host:Logging"), sectionName: "Console")
                                .WriteTo.Console(
                                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}\t{Message:lj}{NewLine}{Exception}");
                        });

                    if (context.HostingEnvironment.IsDevelopment())
                        loggerConfiguration
                            .WriteTo.Logger(debugLoggerConfiguration =>
                            {
                                debugLoggerConfiguration
                                    .ReadFrom.Configuration(context.Configuration.GetSection("MODiX:Host:Logging"), sectionName: "Debug")
                                    .WriteTo.Debug(
                                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}{NewLine}\t{Message:lj}{NewLine}{Exception}");
                            });

                    var seqConfiguration = serviceProvider.GetRequiredService<IOptions<LoggingConfiguration>>().Value.Seq;
                    if (seqConfiguration is not null)
                        loggerConfiguration
                            .WriteTo.Logger(seqLoggerConfiguration => seqLoggerConfiguration
                                .ReadFrom.Configuration(context.Configuration.GetSection("MODiX:Host:Logging"), sectionName: "Seq")
                                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                                .WriteTo.Seq(
                                    apiKey:    seqConfiguration.ApiKey,
                                    serverUrl: seqConfiguration.ServerUrl));
                });

        public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
            => services.Add(services => services.AddOptions<LoggingConfiguration>()
                .Bind(configuration.GetSection("MODiX:Host:Logging"))
                .ValidateDataAnnotations()
                .ValidateOnStartup());
    }
}
