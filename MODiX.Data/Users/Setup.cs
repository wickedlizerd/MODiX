using Microsoft.Extensions.DependencyInjection;

namespace Modix.Data.Users
{
    public static class Setup
    {
        public static IServiceCollection AddUsers(this IServiceCollection services)
            => services.AddScoped<IUsersRepository, UsersRepository>();
    }
}
