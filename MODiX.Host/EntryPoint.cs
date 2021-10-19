using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Structured;
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
                .ConfigureServices(services => services
                    .AddSingleton<ILoggerProvider, MyTestLoggerProvider>())
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
                            })))
                .Build();

            host.Run();
        }
    }

    internal class MyTestLoggerProvider
        : ILoggerProvider
    {
        public MyTestLoggerProvider(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public ILogger CreateLogger(string categoryName)
            => new Logger(_serviceProvider);

        public void Dispose() { }

        private readonly IServiceProvider _serviceProvider;

        private class Logger
            : ILogger
        {
            public Logger(IServiceProvider serviceProvider)
                => _serviceProvider = serviceProvider;

            public IDisposable BeginScope<TState>(TState state)
                => DummyDisposable.Instance;

            public bool IsEnabled(LogLevel logLevel)
                => logLevel is LogLevel.Error;

            public void Log<TState>(
                LogLevel                            logLevel,
                EventId                             eventId,
                TState                              state,
                Exception?                          exception,
                Func<TState, Exception?, string>    formatter)
            {
                if (eventId != _saveChangesFailedEventId)
                    return;

                using (var buffer = new MemoryStream())
                {
                    try
                    {
                        JsonSerializer.Serialize<object>(buffer, state!, new JsonSerializerOptions()
                        {
                            Encoder             = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                            ReferenceHandler    = ReferenceHandler.Preserve
                        });

                        _saveChangesFailedOccurred.Invoke(
                            _serviceProvider.GetRequiredService<ILogger<Logger>>(),
                            Encoding.UTF8.GetString(buffer.GetBuffer().AsSpan(0, (int)buffer.Length)),
                            null);
                    }
                    catch (Exception ex)
                    {
                        _saveChangesFailedOccurred.Invoke(
                            _serviceProvider.GetRequiredService<ILogger<Logger>>(),
                            Encoding.UTF8.GetString(buffer.GetBuffer().AsSpan(0, (int)buffer.Length)),
                            ex);
                    }
                }
            }

            private static readonly EventId _saveChangesFailedEventId
                = new(10000, "Microsoft.EntityFrameworkCore.Update.SaveChangesFailed");

            private static readonly Action<ILogger, string, Exception?> _saveChangesFailedOccurred
                = StructuredLoggerMessage.Define<string>(
                    logLevel:               LogLevel.Error,
                    eventId:                new(0x68D584B3, "SaveChangesFailedOccurred"),
                    formatString:           "SaveChangesFailed has occurred",
                    unformattedValueNames:  "BufferText");

            private readonly IServiceProvider _serviceProvider;

            private sealed class DummyDisposable
                : IDisposable
            {
                public static readonly DummyDisposable Instance
                    = new();

                private DummyDisposable() { }

                public void Dispose() { }
            }
        }
    }
}
