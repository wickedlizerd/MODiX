using Microsoft.Extensions.DependencyInjection;

using Remora.Commands.Extensions;

namespace Modix.Bot.Parsers
{
    public static class Setup
    {
        public static IServiceCollection AddParsers(this IServiceCollection services)
            => services
                .AddParser<TimeSpanParser>();
    }
}
