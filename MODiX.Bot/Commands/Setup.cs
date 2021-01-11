using Microsoft.Extensions.DependencyInjection;

using Remora.Commands.Extensions;

namespace Modix.Bot.Commands
{
    public static class Setup
    {
        public static IServiceCollection AddModixCommands(this IServiceCollection services)
            => services
                .AddCommandGroup<DiagnosticsCommandGroup>();
    }
}
