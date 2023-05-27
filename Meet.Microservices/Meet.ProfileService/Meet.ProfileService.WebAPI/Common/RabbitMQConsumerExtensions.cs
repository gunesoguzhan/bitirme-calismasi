using Meet.Common.RabbitMQ;
using Meet.ProfileService.WebAPI.Consumers;

namespace Meet.ProfileService.WebAPI.Common;

public static class RabbitMQConsumerExtensions
{
    public static IServiceCollection AddRabbitMQConsumers(this IServiceCollection services)
    {
        services.AddScoped<IConsumer, UserRegisteredConsumer>();
        return services;
    }
}