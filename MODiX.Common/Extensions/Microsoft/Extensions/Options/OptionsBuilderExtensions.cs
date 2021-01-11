using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Options
{
    public static class OptionsBuilderExtensions
    {
        public static OptionsBuilder<TOptions> ValidateOnStartup<TOptions>(this OptionsBuilder<TOptions> optionsBuilder)
            where TOptions : class, new()
        {
            optionsBuilder.Services
                .AddStartupAction<OptionsValidationStartupAction<TOptions>>();

            return optionsBuilder;
        }
    }
}
