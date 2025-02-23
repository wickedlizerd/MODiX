﻿using System;
using System.Diagnostics;
using System.IO;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Modix.Data.Models.Core;
using Modix.Services.CodePaste;
using Modix.Services.Utilities;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Modix
{
    public class Program
    {
        public static int Main(string[] args)
        {
            const string DEVELOPMENT_ENVIRONMENT_VARIABLE = "ASPNETCORE_ENVIRONMENT";
            const string DEVELOPMENT_ENVIRONMENT_KEY = "Development";

            var environment = Environment.GetEnvironmentVariable(DEVELOPMENT_ENVIRONMENT_VARIABLE);

            var configBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables("MODIX_")
                .AddJsonFile("developmentSettings.json", optional: true, reloadOnChange: false)
                .AddKeyPerFile("/run/secrets", true);

            if(environment is DEVELOPMENT_ENVIRONMENT_KEY)
            {
                configBuilder.AddUserSecrets<Program>();
            }

            var builtConfig = configBuilder.Build();
            var config = new ModixConfig();
            builtConfig.Bind(config);


            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Modix.DiscordSerilogAdapter", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Logger(subLoggerConfig => subLoggerConfig
                    .MinimumLevel.Information()
                    // .MinimumLevel.Override() is not supported for sub-loggers, even though the docs don't specify this. See https://github.com/serilog/serilog/pull/1033
                    .Filter.ByExcluding("SourceContext like 'Microsoft.%' and @Level in ['Information', 'Debug', 'Verbose']")
                    .WriteTo.Console()
                    .WriteTo.RollingFile(Path.Combine("logs", "{Date}.log")))
                .WriteTo.RollingFile(
                    new RenderedCompactJsonFormatter(),
                    Path.Combine("logs", "{Date}.clef"),
                    retainedFileCountLimit: 2);

            var seqEndpoint = config.SeqEndpoint;
            var seqKey = config.SeqKey;

            if (seqEndpoint != null && seqKey == null) // seq is enabled without a key
            { 
                loggerConfig = loggerConfig.WriteTo.Seq(seqEndpoint);
            }
            else if(seqEndpoint != null && seqKey != null) //seq is enabled with a key
            {
                loggerConfig = loggerConfig.WriteTo.Seq(seqEndpoint, apiKey: seqKey);
            }

            var webhookId = config.LogWebhookId;
            var webhookToken = config.LogWebhookToken;

            var webHost = CreateWebHostBuilder(args, builtConfig).Build();

            if (webhookId.HasValue && webhookToken != null)
            {
                loggerConfig = loggerConfig
                    .WriteTo.DiscordWebhookSink(webhookId.Value, webhookToken, LogEventLevel.Error, webHost.Services.GetRequiredService<CodePasteService>());
            }

            Log.Logger = loggerConfig.CreateLogger();

            try
            {
                webHost.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.ForContext<Program>()
                    .Fatal(ex, "Host terminated unexpectedly.");

                if (Debugger.IsAttached && Environment.UserInteractive)
                {
                    Console.WriteLine(Environment.NewLine + "Press any key to exit...");
                    Console.ReadKey(true);
                }

                return ex.HResult;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseUrls("http://localhost:5003")
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
