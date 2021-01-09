using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Modix.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services
                .AddResponseCompression(options => options
                    .MimeTypes = ResponseCompressionDefaults.MimeTypes
                        .Append("application/octet-stream"));

        public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            application
                .UseResponseCompression();

            if (environment.IsDevelopment())
                application
                    .UseDeveloperExceptionPage()
                    .UseWebAssemblyDebugging();

            application
                .UseBlazorFrameworkFiles()
                .UseStaticFiles()
                .UseFallbackFile("index.html");
        }
    }
}
