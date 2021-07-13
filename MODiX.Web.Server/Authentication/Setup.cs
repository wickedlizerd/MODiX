using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Server.Authentication
{
    public static class Setup
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
            => services
                .Add(services => services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateAudience            = false,
                            ValidateLifetime            = true,
                            ValidateIssuer              = false,
                            ValidateIssuerSigningKey    = true
                        };
                    }))
                .PostConfigureWith<JwtBearerOptions, IOptions<AuthenticationConfiguration>>((options, authenticationConfiguration) => options
                    .TokenValidationParameters
                        .IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenticationConfiguration.Value.TokenSignatureSecret)))
                .AddScopedWithAlias<AuthenticationContract, IAuthenticationContract>()
                .Add(services => services.AddOptions<AuthenticationConfiguration>()
                    .Bind(configuration.GetSection("MODiX:Web:Server:Authentication"))
                    .ValidateDataAnnotations()
                    .ValidateOnStartup());

        public static IEndpointRouteBuilder MapAuthentication(this IEndpointRouteBuilder endpoints)
            => endpoints
                .Map(endpoints => endpoints.MapGrpcService<AuthenticationContract>());
    }
}
