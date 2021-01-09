using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseFallbackFile(
                this IApplicationBuilder application,
                string filePath)
            => application
                .Use(next => context =>
                {
                    // I know this seems hacky, but it's copy/pasted straight out of ASP.NET Core source code, and that's good enough for me.
                    // https://github.com/dotnet/aspnetcore/blob/b0a6755b5ef103b57394f115613b45a37912600a/src/Middleware/StaticFiles/src/StaticFilesEndpointRouteBuilderExtensions.cs#L194
                    context.Request.Path = "/" + filePath;

                    // StaticFilesMiddleware won't handle endpoints
                    context.SetEndpoint(null);

                    return next(context);
                })
                .UseStaticFiles();
    }
}
