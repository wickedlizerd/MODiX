using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Modix.Business.Diagnostics;

namespace Modix.Business
{
    public static class Setup
    {
        public static IServiceCollection AddModixBusiness(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDiagnostics(configuration);
    }
}
