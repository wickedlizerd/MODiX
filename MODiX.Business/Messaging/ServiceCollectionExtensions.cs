using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace Modix.Business.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationHandler<THandler>(this IServiceCollection services)
                where THandler : class, INotificationHandler
            => services.AddNotificationHandler(typeof(THandler));

        public static IServiceCollection AddNotificationHandler(this IServiceCollection services, Type handlerType)
        {
            var handlerInterfaceTypes = handlerType
                .GetInterfaces()
                .Where(type => type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                .ToArray();

            if (handlerInterfaceTypes.Length == 0)
                throw new ArgumentException($"Handler does not implement {nameof(INotificationHandler)}<>", nameof(handlerType));
            else if (handlerInterfaceTypes.Length == 1)
                services.AddTransient(handlerInterfaceTypes[0], handlerType);
            else
            {
                services.AddTransient(handlerType);
                foreach (var handlerInterfaceType in handlerInterfaceTypes)
                    services.AddTransient(handlerInterfaceType, serviceProvider => serviceProvider.GetRequiredService(handlerType));
            }

            return services;
        }
    }
}
