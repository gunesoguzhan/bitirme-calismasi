using Meet.IdentityService.WebAPI.Producers;

namespace Meet.IdentityService.WebAPI.Common;

public static class RabbitMQProducerExtensions
{
    public static IServiceCollection AddRabbitMQProducers(this IServiceCollection services)
    {
        services.AddSingleton<UserRegisteredProducer>();
        return services;
    }
}