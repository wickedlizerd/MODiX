using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

using ProtoBuf.Grpc.Client;

using Modix.Web.Protocol.Authentication;

namespace Modix.Web.Client.Authentication
{
    public static class AuthenticationSetup
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services)
            => services
                .AddSingletonWithAliases<AuthenticationManager, IAuthenticationManager, AuthenticationStateProvider>()
                .AddContractClient<IAuthenticationContract>()
                .AddTransient<LoginPageModel>()
                .AddTransient<LogoutPageModel>();
    }
}
