using Microsoft.Extensions.DependencyInjection;

namespace Modix.Bot.Controls
{
    public static class Setup
    {
        public static IServiceCollection AddControls(this IServiceCollection services)
            => services
                .AddScoped<IMessageDialogFactory, MessageDialogFactory>()
                .AddScoped<IReactionButtonFactory, ReactionButtonFactory>();
    }
}
