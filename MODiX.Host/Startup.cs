using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Modix.Bot;
using Modix.Business;

namespace Modix.Host
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
            => services
                .AddResponseCompression(options => options
                    .MimeTypes = ResponseCompressionDefaults.MimeTypes
                        .Append("application/octet-stream"))
                .AddModixBot(_configuration)
                .AddModixBusiness(_configuration);

        public void Configure(IApplicationBuilder application)
        {
            application
                .UseResponseCompression();

            if (_webHostEnvironment.IsDevelopment())
                application
                    .UseDeveloperExceptionPage()
                    .UseWebAssemblyDebugging();

            application
                .UseBlazorFrameworkFiles()
                .UseStaticFiles()
                .UseFallbackFile("index.html");
        }

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
    }
}
