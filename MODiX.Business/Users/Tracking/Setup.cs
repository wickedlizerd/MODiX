using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Modix.Business.Messaging;

namespace Modix.Business.Users.Tracking
{
    public static class Setup
    {
        public static IServiceCollection AddUserTracking(this IServiceCollection services, IConfiguration configuration)
            => services
                .Add(services => services.AddOptions<UserTrackingConfiguration>()
                    .Bind(configuration.GetSection("MODiX:Business:Users:Tracking"))
                    .ValidateDataAnnotations()
                    .ValidateOnStartup())
                .AddSingleton<IUserTrackingCache, UserTrackingCache>()
                .AddNotificationHandler<UserTrackingNotificationHandler>()
                .AddHostedService<UserTrackingCacheCleaningBehavior>()
                .AddScoped<IUserTrackingService, UserTrackingService>();
    }
}
