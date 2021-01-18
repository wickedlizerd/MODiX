using System;

namespace Microsoft.AspNetCore.Routing
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder Map(
            this IEndpointRouteBuilder endpoints,
            Action<IEndpointRouteBuilder> onMapping)
        {
            onMapping.Invoke(endpoints);

            return endpoints;
        }
    }
}
