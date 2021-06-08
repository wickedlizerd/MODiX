using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

namespace ProtoBuf.Grpc.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContractClient<TContract>(this IServiceCollection services)
                where TContract : class
            => services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<GrpcChannel>().CreateGrpcService<TContract>());
    }
}
